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
			
			FluentResource resource;
			try
			{
				var content = File.ReadAllText(file);
				resource = FluentDeserializer.Deserialize(content);
			}
			catch (Exception ex)
			{
				return;
			}
			
			if (resource is null)
				return;

			var currentEntries = resource.Entries
				.OfType<FluentRecord>()
				.ToDictionary(x => x.Reference);

			var records = _database.GetCollection<TranslationRecord>();
			var previousEntries = records.Query()
				.Where(x => x.Path == file)
				.ToEnumerable()
				.ToDictionary(x => x.Name);



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