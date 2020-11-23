using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FluentTranslate.WebHost
{
	public class FluentFileGeneratorService : BackgroundService
	{
		public IHostEnvironment HostEnvironment { get; }

		private readonly IOptionsMonitor<FluentTranslateOptions> _optionsMonitor;
		private readonly Subject<int> _updateSubject = new Subject<int>();

		private FileSystemWatcher _watcher;
		private IDisposable _optionsSubscription;
		private IDisposable _updateSubscription;


		public FluentFileGeneratorService(IHostEnvironment hostEnvironment, IOptionsMonitor<FluentTranslateOptions> optionsMonitor)
		{
			HostEnvironment = hostEnvironment;

			_optionsMonitor = optionsMonitor;	
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var options = _optionsMonitor.CurrentValue;

			_watcher = new FileSystemWatcher()
			{
				Path = options.SourceFilesPath,
				IncludeSubdirectories = true,
			};
			_watcher.Changed += (sender, args) => _updateSubject.OnNext(0);
			_watcher.Created += (sender, args) => _updateSubject.OnNext(0);
			_watcher.Deleted += (sender, args) => _updateSubject.OnNext(0);
			_watcher.Renamed += (sender, args) => _updateSubject.OnNext(0);

			_watcher.EnableRaisingEvents = true;

			_optionsSubscription = _optionsMonitor.OnChange(_ => _updateSubject.OnNext(0));

			_updateSubscription = _updateSubject
				.Throttle(TimeSpan.FromSeconds(1))
				.Subscribe(_ => UpdateGeneratedFiles());

			UpdateGeneratedFiles();

			while (!stoppingToken.IsCancellationRequested)
			{
				await Task.Delay(2000, stoppingToken);
			}
		}

		private void UpdateGeneratedFiles()
		{
			var options = _optionsMonitor.CurrentValue;

			var sourceFilesPath = options.SourceFilesPath;
			var generatedFilesPath = options.GeneratedFilesPath;
			var sources = options.GenerateFiles
				.SelectMany(x => x.Sources)
				.Select(x => Path.IsPathRooted(x) ? x : Path.Combine(sourceFilesPath, x))
				.ToHashSet();

			var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
		}

		private void Cleanup()
		{
			var watcher = Interlocked.Exchange(ref _watcher, null);
			watcher?.Dispose();
			
			var optionsSub = Interlocked.Exchange(ref _optionsSubscription, null);
			optionsSub?.Dispose();

			var updateSub = Interlocked.Exchange(ref _updateSubscription, null);
			updateSub?.Dispose();
		}

		public override void Dispose()
		{
			Cleanup();

			base.Dispose();
		}
	}
}
