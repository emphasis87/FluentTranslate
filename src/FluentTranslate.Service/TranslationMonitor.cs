using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentTranslate.Common.Domain;
using FluentTranslate.Parser;
using LiteDB;

namespace FluentTranslate.Service
{
	public interface ITranslationMonitor
	{
		void Add(string path, string filter = "*.flt");
	}

	public class TranslationMonitor : IDisposable
	{
		private readonly LiteDatabase _database;

		private readonly List<FileSystemWatcher> _watchers = 
			new List<FileSystemWatcher>();

		public TranslationMonitor(LiteDatabase database)
		{
			_database = database;
		}

		public void Add(string path, string filter = "*.flt")
		{
			if (path is null) 
				throw new ArgumentNullException(nameof(path));
			if (filter is null) 
				throw new ArgumentNullException(nameof(filter));

			path = Path.GetFullPath(path);

			if (Directory.Exists(path))
			{
				var files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);
				foreach (var file in files)
					Parse(file);
			}

			lock (_watchers)
			{
				if (_watchers.Any(x => x.Path == path && x.Filter == filter))
					return;

				var watcher = new FileSystemWatcher(path, filter);
				watcher.Created += Watcher_Created;
				watcher.Changed += Watcher_Changed;
				watcher.Deleted += Watcher_Deleted;
				watcher.Renamed += Watcher_Renamed;
				_watchers.Add(watcher);
			}
		}

		private void Watcher_Changed(object sender, FileSystemEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Watcher_Deleted(object sender, FileSystemEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Watcher_Created(object sender, FileSystemEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Watcher_Renamed(object sender, RenamedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Parse(string file)
		{
			if (file is null) 
				throw new ArgumentNullException(nameof(file));

            DateTime lastModified;
			FluentResource resource;
			try
			{
				var content = File.ReadAllText(file);
                lastModified = File.GetLastWriteTimeUtc(file);
				resource = FluentDeserializer.Deserialize(content);
			}
			catch (Exception ex)
			{
				return;
			}
			
			if (resource is null)
				return;

			var attributeLevels = new List<IDictionary<string, object>>();
			attributeLevels.AddRange(Enumerable.Repeat(default(IDictionary<string, object>), 4));

            var currentEntries = new List<FluentRecord>();
            foreach (var entry in resource.Entries)
            {
                switch (entry)
                {
					case FluentComment comment:
                    {
                        var attributes = ParseAttributes(comment.Value);
                        if (attributes != null)
                            attributeLevels[comment.Level] = attributes;
                        break;
                    }
                    case FluentRecord record:
                    {
                        var attributes = ParseAttributes(record.Comment);
                        attributeLevels[1] = attributes;
						var attributeResult = new Dictionary<string, object>();
                        foreach (var attributeLevel in attributeLevels.Where(x => x != null))
                        {
                            foreach (var attribute in attributeLevel)
                            {
                                if (!attributeResult.ContainsKey(attribute.Key))
                                    attributeResult.Add(attribute.Key, attribute.Value);
                            }
                        }
                        break;
                    }
                }
            }

            var currentRecords = resource.Entries
                .OfType<FluentRecord>()
                .ToDictionary(x => x.Reference);

			var records = _database.GetCollection<TranslationRecord>();
			var previousEntries = records.Query()
				.Where(x => x.Path == file)
				.ToEnumerable()
				.ToDictionary(x => x.Name);

            foreach (var previous in previousEntries)
            {
                var previousRecord = previous.Value;
				if (!currentEntries.TryGetValue(previous.Key, out var currentRecord))
                {
                    if (previousRecord.Content != null)
                        previousRecord.RemovedAt(lastModified);
                }
				else
                {
                    var currentContent = Utf8Json.JsonSerializer.ToJsonString(currentRecord);
					if (previousRecord.Content != currentRecord.)
                }
            }

		}

        private IDictionary<string, object> ParseAttributes(string comment)
        {

        }

		public void Dispose()
		{
			lock (_watchers)
			{
				foreach (var watcher in _watchers)
					watcher.Dispose();

				_watchers.Clear();
			}
		}
	}
}