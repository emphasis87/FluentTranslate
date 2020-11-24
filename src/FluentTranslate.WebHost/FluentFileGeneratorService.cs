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
using FluentTranslate.WebHost.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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

			UpdateGeneratedFiles();

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
			var sourceFiles = Directory.GetFiles(sourceFilesPath, "*",
				new EnumerationOptions() {RecurseSubdirectories = true});
			
			var sourceContexts = sourceFiles
				.Select(GetSourceFileContext)
				.ToArray();
			var sourceContextsByInvariantPath = sourceContexts
				.ToLookup(x => x.InvariantPath);
			var sourceInvariantPaths = sourceContexts
				.SelectMany(x => new []{x.InvariantPath, x.Path})
				.Select(path => path[(sourceFilesPath.Length + 1)..])
				.Distinct()
				.ToArray();

			foreach (var generateContext in generateContexts)
			{
				
			}
		}

		private GenerateFileContext GetGenerateFileContext(FluentGenerateFileOptions generate)
		{
			if (_generateContextByPath.TryGetValue(generate.Name, out var context))
				return context;

			context = CreateGenerateFileContext(generate);
			context = _generateContextByPath.GetOrAdd(generate.Name, context);
			return context;
		}

		private GenerateFileContext CreateGenerateFileContext(FluentGenerateFileOptions generate)
		{
			var path = generate.Name;
			var sources = generate.Sources;
			var context = new GenerateFileContext(path, sources);
			return context;
		}

		private SourceFileContext GetSourceFileContext(string path)
		{
			if (_sourceContextByPath.TryGetValue(path, out var context)) 
				return context;

			context = CreateSourceFileContext(path);
			context = _sourceContextByPath.GetOrAdd(path, context);
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

			var context = new SourceFileContext(path, invariantPath, cultures.ToImmutableHashSet());
			return context;
		}

		private class SourceFileContext
		{
			public string Path { get; }
			public string InvariantPath { get; }
			public ImmutableHashSet<string> Cultures { get; }

			public FluentResource LastResult;
			public DateTime? LastModified;

			public SourceFileContext(string path, string invariantPath, ImmutableHashSet<string> cultures)
			{
				Path = path;
				InvariantPath = invariantPath;
				Cultures = cultures;
			}
		}

		private class GenerateFileContext
		{
			public string Path { get; }
			public ImmutableList<string> Sources { get; }

			public DateTime? LastModified;

			public GenerateFileContext(string path, IEnumerable<string> sources)
			{
				Path = path;
				Sources = sources.ToImmutableList();
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
	}
}
