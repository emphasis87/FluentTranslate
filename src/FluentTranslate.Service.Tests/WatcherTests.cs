using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FluentTranslate.Service.Tests
{
	public class WatcherTests
	{
		[Test]
		public async Task Debounce_window()
		{
			var wd = TestContext.CurrentContext.WorkDirectory;

			foreach (var file in Directory.GetFiles(wd, "*.txt"))
			{
				File.Delete(file);
			}

			Console.WriteLine($"{DateTime.UtcNow:O}");

			var watcher = new FileSystemWatcher(wd)
			{
				EnableRaisingEvents = true
			};

			var changed = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
					h => watcher.Changed += h,
					h => watcher.Changed -= h)
				.Select(x => x.EventArgs.Name);
			var deleted = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
					h => watcher.Deleted += h,
					h => watcher.Deleted -= h)
				.Select(x => x.EventArgs.Name);
			var renamed = Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
					h => watcher.Renamed += h,
					h => watcher.Renamed -= h)
				.SelectMany(e => new[] {e.EventArgs.OldName,e.EventArgs.Name});

			// Buffer events until there is a certain interval of inactivity
			var events = Observable.Merge(changed, deleted, renamed);
			var subscription = events
				.Do(next =>
					Console.WriteLine($"{DateTime.UtcNow:O} {next}"))
				.Buffer(() => 
					events
						.Throttle(TimeSpan.FromSeconds(2)))
				.Select(x => 
					x.ToHashSet())
				.Timestamp()
				.Subscribe(next =>
				{
					Console.WriteLine($"{next.Timestamp:O}\r\n\t{string.Join(", ", next.Value.OrderBy(x => x))}");
				});

			await Task.Delay(1000);

			File.Create(Path.Combine(wd, "a.txt")).Close();
			File.WriteAllText(Path.Combine(wd, "a.txt"), "A");
			File.Copy(Path.Combine(wd, "a.txt"), "b.txt");
			File.Move(Path.Combine(wd, "a.txt"), "c.txt");

			await Task.Delay(3000);

			File.Move(Path.Combine(wd, "c.txt"), "a.txt");

			await Task.Delay(1000);

			File.Move(Path.Combine(wd, "a.txt"), "c.txt");

			await Task.Delay(1000);

			File.Move(Path.Combine(wd, "c.txt"), "a.txt");

			await Task.Delay(1000);

			File.Move(Path.Combine(wd, "a.txt"), "c.txt");

			await Task.Delay(3000);

			subscription.Dispose();
		}
	}
}
