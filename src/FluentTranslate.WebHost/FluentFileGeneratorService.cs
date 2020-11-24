using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using FluentTranslate.Domain;
using FluentTranslate.Infrastructure;
using FluentTranslate.WebHost.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using static FluentTranslate.WebHost.Infrastructure.EqualityHelper;

namespace FluentTranslate.WebHost
{
	public class FluentFileGeneratorService : BackgroundService
	{
		private readonly IOptionsMonitor<FluentTranslateOptions> _optionsMonitor;

		private readonly Subject<int> _sourceSubject = new Subject<int>();
		private readonly Subject<FluentTranslateOptions> _optionsSubject = new Subject<FluentTranslateOptions>();

		private FileSystemWatcher _sourceWatcher;
		
		private IDisposable _subscriptions;

		private readonly ConcurrentDictionary<string, GenerateFileContext> _generateContextByPath = 
			new ConcurrentDictionary<string, GenerateFileContext>();

		private readonly ConcurrentDictionary<string, SourceFileContext> _sourceContextByPath =
			new ConcurrentDictionary<string, SourceFileContext>();

		private ISet<string> _cultureNames;
		private ISet<string> _cultureTwoLetterIsoNames;

		protected IFluentMerger Merger => FluentMerger.Default;
		protected IFluentEqualityComparer EqualityComparer => FluentEqualityComparer.Default;

		public FluentFileGeneratorService(IOptionsMonitor<FluentTranslateOptions> optionsMonitor)
		{
			_optionsMonitor = optionsMonitor;	
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var options = _optionsMonitor.CurrentValue;

			_sourceWatcher = new FileSystemWatcher()
			{
				Path = options.SourceFilesPath,
				IncludeSubdirectories = true,
			};
			_sourceWatcher.Changed += (sender, args) => _sourceSubject.OnNext(0);
			_sourceWatcher.Created += (sender, args) => _sourceSubject.OnNext(0);
			_sourceWatcher.Deleted += (sender, args) => _sourceSubject.OnNext(0);
			_sourceWatcher.Renamed += (sender, args) => _sourceSubject.OnNext(0);

			_sourceWatcher.EnableRaisingEvents = true;

			// Monitor options changes, report only distinct generated files
			var optionsMonitorSubscription = _optionsMonitor
				.OnChange(opt => _optionsSubject.OnNext(opt));
			var optionChanges = _optionsSubject
				.DistinctUntilChanged(FluentFileGeneratorOptionsEqualityComparer.Default)
				.Select(_ => 0);

			// Throttle updating generated files to 5s
			var updateSubscription = _sourceSubject.Merge(optionChanges)
				.Throttle(TimeSpan.FromSeconds(5))
				.Subscribe(_ => UpdateGeneratedFiles());
			
			_subscriptions = new CompositeDisposable(
				optionsMonitorSubscription, updateSubscription);

			var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
			_cultureNames = cultures.Select(x => x.Name).ToImmutableHashSet();
			_cultureTwoLetterIsoNames = cultures.Select(x => x.TwoLetterISOLanguageName).ToImmutableHashSet();

			_optionsSubject.OnNext(options);

			while (!stoppingToken.IsCancellationRequested)
			{
				await Task.Delay(TimeSpan.FromSeconds(4), stoppingToken);
			}
		}
		
		private void UpdateGeneratedFiles()
		{
			var options = _optionsMonitor.CurrentValue;
			var sourceFilesPath = options.SourceFilesPath;

			// Get context for generated files
			var generateFiles = options.GenerateFiles;
			var generateContexts = generateFiles
				.Select(GetGenerateFileContext)
				.ToArray();
			// Remove old generated files
			var generatePaths = generateFiles.Select(gen => gen.Name).ToArray();
			foreach (var generatePath in _generateContextByPath.Keys.Except(generatePaths))
			{
				_generateContextByPath.TryRemove(generatePath, out _);
			}

			// Get context for source files
			string[] sourceFiles;
			try
			{
				sourceFiles = Directory.GetFiles(sourceFilesPath, "*",
					new EnumerationOptions() {RecurseSubdirectories = true});
			}
			catch (Exception)
			{
				sourceFiles = new string[0];
			}

			var sourceContexts = sourceFiles
				.Select(GetSourceFileContext)
				.ToArray();

			// Reload changed resources
			foreach (var sourceContext in sourceContexts.Where(x => x.HasChanged))
			{
				sourceContext.Result = DeserializeFile(sourceContext.Path);
			}

			var sourceContextsByInvariantPath = sourceContexts
				.ToLookup(x => x.InvariantPath);
			var sourceContextsByPath = sourceContexts
				.ToDictionary(x => x.Path);
			var sourcePaths = sourceContexts
				.SelectMany(x => new[] {x.InvariantPath, x.Path})
				.Distinct()
				.Select(path => (relative: path[(sourceFilesPath.Length + 1)..], absolute: path))
				.ToArray();

			foreach (var generateContext in generateContexts)
			{
				// Find source paths
				var sources = new List<string>();
				foreach (var source in generateContext.Sources)
				{
					if (Path.IsPathRooted(source))
					{
						sources.Add(source);
						continue;
					}

					foreach (var (sourceRelativePath, sourceAbsolutePath) in sourcePaths)
					{
						if (sourceRelativePath.EndsWith(source))
							sources.Add(sourceAbsolutePath);
					}
				}

				// Find source contexts
				var currentContexts = MoreLinq.MoreEnumerable.DistinctBy(sources
						.Distinct()
						.SelectMany(source =>
						{
							if (sourceContextsByInvariantPath.Contains(source))
								return sourceContextsByInvariantPath[source].OrderByDescending(x => x.Path).ToArray();
							if (sourceContextsByPath.ContainsKey(source))
								return new[] {sourceContextsByPath[source]};
							return new SourceFileContext[0];
						}), x => x.Path)
					.ToArray();

				UpdateGeneratedFile(generateContext, currentContexts);
			}
		}

		private void UpdateGeneratedFile(GenerateFileContext generateContext, SourceFileContext[] sourceContexts)
		{
			var nextPaths = sourceContexts.Select(x => x.Path).ToArray();
			if (generateContext.SourceContexts != null)
			{
				var prevPaths = generateContext.SourceContexts.Select(x => x.Path).ToArray();
				if (AreEqual(prevPaths, nextPaths) &&
					sourceContexts.All(x => !x.HasChanged))
					return; // No changes
			}

			generateContext.SourceContexts = sourceContexts;
			generateContext.Cultures = sourceContexts.SelectMany(x => x.Cultures).ToHashSet();

			foreach (var culture in generateContext.Cultures.Concat(new string[] {null}))
			{
				var path = generateContext.Path;
				if (culture != null)
				{
					var extension = Path.GetExtension(path);
					path = Path.ChangeExtension(path, $".{culture}{extension}");
				}

				var prevResult = DeserializeFile(path);
				var sources = sourceContexts.Where(x => x.Cultures.Count == 0 || x.Cultures.Contains(culture)).ToArray();
				var results = sources.Select(x => x.Result).ToArray();
				var result = Merger.Combine(results);
				if (!EqualityComparer.Equals(prevResult, result))
					SerializeFile(path, result);
			}
		}

		private GenerateFileContext GetGenerateFileContext(FluentGenerateFileOptions generate)
		{
			if (!_generateContextByPath.TryGetValue(generate.Name, out var context))
			{
				context = CreateGenerateFileContext(generate);
				context = _generateContextByPath.GetOrAdd(generate.Name, context);
			}

			context.Sources = generate.Sources;

			return context;
		}

		private GenerateFileContext CreateGenerateFileContext(FluentGenerateFileOptions generate)
		{
			var path = generate.Name;
			var context = new GenerateFileContext(path);
			return context;
		}

		private SourceFileContext GetSourceFileContext(string path)
		{
			if (!_sourceContextByPath.TryGetValue(path, out var context))
			{
				context = CreateSourceFileContext(path);
				context = _sourceContextByPath.GetOrAdd(path, context);
			}

			context.PrevModified = context.LastModified;
			try
			{
				context.LastModified = File.GetLastWriteTime(path);
			}
			catch (Exception)
			{
				context.LastModified = null;
			}

			return context;
		}

		private SourceFileContext CreateSourceFileContext(string path)
		{
			// Find the invariant path and cultures specified in the file name
			var invariantPath = path;
			var cultures = new HashSet<string>();
			var extensions = new Stack<string>();
			string extension;
			while (!string.IsNullOrEmpty(extension = Path.GetExtension(invariantPath)))
			{
				extension = extension.TrimStart('.');

				if (_cultureTwoLetterIsoNames.Contains(extension))
				{
					cultures.Add(extension);
				}
				else if (_cultureNames.Contains(extension))
				{
					cultures.Add(extension.Substring(0, 2));
					cultures.Add(extension);
				}
				else
				{
					extensions.Push(extension);
				}

				invariantPath = Path.ChangeExtension(invariantPath, null);
			}

			while (extensions.TryPop(out extension))
			{
				invariantPath = $"{invariantPath}.{extension}";
			}

			var context = new SourceFileContext(path, invariantPath, cultures.ToHashSet());
			return context;
		}

		private class SourceFileContext
		{
			public string Path { get; }
			public string InvariantPath { get; }
			public HashSet<string> Cultures { get; }
			public DateTime? PrevModified { get; set; }
			public DateTime? LastModified { get; set; }
			public FluentResource Result { get; set; }
			public bool HasChanged => PrevModified != LastModified;

			public SourceFileContext(string path, string invariantPath, HashSet<string> cultures)
			{
				Path = path;
				InvariantPath = invariantPath;
				Cultures = cultures;
			}
		}

		private class GenerateFileContext
		{
			public string Path { get; }
			public string[] Sources { get; set; }
			public SourceFileContext[] SourceContexts { get; set; }
			public HashSet<string> Cultures { get; set; }
			
			public GenerateFileContext(string path)
			{
				Path = path;
			}
		}

		public override void Dispose()
		{
			var watcher = Interlocked.Exchange(ref _sourceWatcher, null);
			watcher?.Dispose();

			var subscriptions = Interlocked.Exchange(ref _subscriptions, null);
			subscriptions?.Dispose();

			base.Dispose();
		}

		private static FluentResource DeserializeFile(string path)
		{
			if (!File.Exists(path))
				return null;

			try
			{
				var content = File.ReadAllText(path);
				var resource = FluentConverter.Deserialize(content);
				return resource;
			}
			catch (Exception)
			{
				return null;
			}
		}

		private static void SerializeFile(string path, FluentResource resource)
		{
			try
			{
				var directory = Path.GetDirectoryName(path);
				if (!Directory.Exists(directory))
					Directory.CreateDirectory(directory);

				var content = FluentConverter.Serialize(resource);
				File.WriteAllText(path, content);
			}
			catch (Exception)
			{
				
			}
		}
	}
}
