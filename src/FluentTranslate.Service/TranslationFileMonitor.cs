using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using FluentTranslate.Common.Domain;
using FluentTranslate.Parser;
using FluentTranslate.Service.Domain;
using LiteDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FluentTranslate.Service
{
	public interface ITranslationFileMonitor
	{
		void Add(string path, string filter = "*.flt");
	}

	public class TranslationFileMonitor : ITranslationFileMonitor, IDisposable
	{
		private readonly LiteDatabase _database;
		private readonly IOptions<Configuration> _configuration;
		private readonly ILogger<TranslationFileMonitor> _logger;

		private readonly List<FileSystemWatcher> _watchers = 
			new List<FileSystemWatcher>();

		private readonly List<IDisposable> _subscriptions =
			new List<IDisposable>();

		protected TimeSpan MonitoringDebounceInterval =>
			_configuration?.Value.MonitoringDebounceInterval ?? TimeSpan.FromSeconds(60);

		public TranslationFileMonitor(
			LiteDatabase database,
			IOptions<Configuration> configuration = null,
			ILogger<TranslationFileMonitor> logger = null)
		{
			_database = database;
			_configuration = configuration;
			_logger = logger;
		}

		public void Add(string path, string filter = "*.flt")
		{
			if (path is null) 
				throw new ArgumentNullException(nameof(path));
			if (filter is null) 
				throw new ArgumentNullException(nameof(filter));

			path = Path.GetFullPath(path);

			lock (_watchers)
			{
				if (_watchers.Any(x => x.Path == path && x.Filter == filter))
					return;

				var watcher = new FileSystemWatcher(path, filter)
				{
					EnableRaisingEvents = true
				};

				var changed = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
						h => watcher.Changed += h,
						h => watcher.Changed -= h)
					.Select(x => x.EventArgs.FullPath);
				var deleted = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
						h => watcher.Deleted += h,
						h => watcher.Deleted -= h)
					.Select(x => x.EventArgs.FullPath);
				var renamed = Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
						h => watcher.Renamed += h,
						h => watcher.Renamed -= h)
					.SelectMany(e => new[] {e.EventArgs.OldFullPath, e.EventArgs.FullPath});

				// Buffer events until there is a certain interval of inactivity
				var events = Observable.Merge(changed, deleted, renamed);
				var subscription = events
					.Buffer(() => events.Throttle(MonitoringDebounceInterval))
					.Select(x => x.ToHashSet())
					.Subscribe(ParseFiles);

				_watchers.Add(watcher);
				_subscriptions.Add(subscription);
			}

			if (Directory.Exists(path))
			{
				var paths = _database.GetCollection<TranslationEntry>().Query()
					.Select(x => x.Path)
					.ToList();
				var files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);
				var result = paths.Concat(files).ToHashSet();
				ParseFiles(result);
			}
		}

		private void ParseFiles(IEnumerable<string> files)
		{
			var hasErrors = false;
			var removedFiles = new List<string>();
			var updatedFiles = new List<string>();
			var entries = new List<TranslationEntry>();
			foreach (var file in files)
			{
				if (!File.Exists(file))
				{
					removedFiles.Add(file);
					continue;
				}

				updatedFiles.Add(file);

				using var loggerScope = _logger.BeginScope(new { File = file });

				string content;
				DateTime lastModified;
				try
				{
					content = File.ReadAllText(file);
					lastModified = File.GetLastWriteTimeUtc(file);
				}
				catch (Exception ex)
				{
					_logger.LogError("Error while reading fluent resource file.", ex);
					hasErrors = true;
					continue;
				}

				FluentResource resource;
				try
				{
					resource = FluentDeserializer.Deserialize(content);
				}
				catch (Exception ex)
				{
					_logger.LogError("Error while deserializing fluent resource file.", ex);
					hasErrors = true;
					continue;
				}

				if (resource is null || resource.Entries.Count == 0)
					continue;

				var currentEntries = ParseResource(file, lastModified, resource);
				var currentEntriesByName = currentEntries.ToLookup(x => x.Name);
				foreach (var grouping in currentEntriesByName)
				{
					var name = grouping.Key;
					var items = grouping.ToList();
					if (items.Count > 1)
					{
						_logger.LogError("Ambiguous definition of entry {Name}", name);
						hasErrors = true;
					}
				}

				entries.AddRange(currentEntries);
			}

			if (hasErrors)
				return;

			_database.GetCollection<TranslationEntry>()
				.DeleteMany(x => removedFiles.Contains(x.Path));

			var entrySet = entries.ToHashSet(new TranslationEntryEqualityComparer());
			_database.GetCollection<TranslationEntry>()
				.DeleteMany(x => updatedFiles.Contains(x.Path) && !entrySet.Contains(x));

			_database.GetCollection<TranslationEntry>()
				.Upsert(entrySet);

			var lists = new Dictionary<(string, object), TranslationList>();
			foreach (var entry in entrySet)
			{
				foreach (var (key, value) in entry.Attributes)
				{
					if (lists.TryGetValue((key, value), out var list))
					{
						list.LastModified = new DateTime(Math.Max(list.LastModified.Ticks, entry.LastModified.Ticks));
						continue;
					}
					list = new TranslationList();
					list.Attributes.Add(key, value);
					list.LastModified = entry.LastModified;
					list.UpdateId();
					lists.Add((key, value), list);
				}
			}
			var mainList = new TranslationList();
			mainList.UpdateId();
			mainList.LastModified = entrySet.Max(x => x.LastModified);

			_database.GetCollection<TranslationList>()
				.Upsert(lists.Values.Concat(new[] {mainList}));
		}

		private static IDictionary<string, object> AggregateDictionaries(IEnumerable<IDictionary<string, object>> dictionaries)
		{
			var result = new Dictionary<string, object>();
			foreach (var dictionary in dictionaries.Where(x => x != null))
			{
				foreach (var (key, value) in dictionary)
				{
					if (!result.ContainsKey(key))
						result.Add(key, value);
				}
			}

			return result;
		}

		private IList<TranslationEntry> ParseResource(string path, DateTime lastModified, FluentResource resource)
		{
			var attributeLevels = new List<IDictionary<string, object>> {null, null, null, null};
			var records = new List<TranslationEntry>();
			foreach (var fluentEntry in resource.Entries)
			{
				switch (fluentEntry)
				{
					case FluentComment fluentComment:
					{
						var attributes = ParseAttributes(fluentComment.Value);
						if (attributes != null)
							attributeLevels[fluentComment.Level] = attributes;
						break;
					}
					case FluentRecord fluentRecord:
					{
						var attributes = ParseAttributes(fluentRecord.Comment);
						attributeLevels[1] = attributes;
						var attributeResult = AggregateDictionaries(attributeLevels);
						var translationEntry = new TranslationEntry(attributeResult)
						{
							Name = fluentRecord.Reference,
							Path = path,
							Content = Utf8Json.JsonSerializer.ToJsonString(fluentRecord),
							LastModified = lastModified,
						};
						translationEntry.UpdateId();
						translationEntry.UpdateETag();
						records.Add(translationEntry);
						break;
					}
				}
			}
			return records;
		}

        private IDictionary<string, object> ParseAttributes(string comment)
		{
			return null;
		}

		public void Dispose()
		{
			lock (_watchers)
			{
				foreach (var watcher in _watchers)
				{
					watcher.Dispose();
				}
				_watchers.Clear();
				foreach (var subscription in _subscriptions)
				{
					subscription.Dispose();
				}
				_subscriptions.Clear();
			}
		}
	}
}