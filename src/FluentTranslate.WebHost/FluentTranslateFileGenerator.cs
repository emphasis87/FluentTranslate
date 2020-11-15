using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FluentTranslate.WebHost
{
	public class FluentTranslateFileGenerator : BackgroundService
	{
		private readonly IOptionsMonitor<FluentTranslateOptions> _optionsMonitor;
		private readonly IDisposable _optionsOnChange;

		public FluentTranslateFileGenerator(IOptionsMonitor<FluentTranslateOptions> optionsMonitor)
		{
			_optionsMonitor = optionsMonitor;
			_optionsOnChange = optionsMonitor.OnChange(OnOptionsChanged);
		}

		private void OnOptionsChanged(FluentTranslateOptions options)
		{
			
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			OnOptionsChanged(_optionsMonitor.CurrentValue);

			return Task.CompletedTask;
		}

		public override void Dispose()
		{
			_optionsOnChange.Dispose();

			base.Dispose();
		}
	}
}
