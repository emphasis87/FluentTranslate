using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FluentTranslate.Domain;
using FluentTranslate.Infrastructure;

namespace FluentTranslate
{
	public interface IFluentClient
	{
		Task<string> Translate(string message, IDictionary<string, object> parameters = null, CultureInfo culture = null);
	}

	public class FluentClient : IFluentClient
	{
		protected SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1, 1);

		protected IFluentConfiguration Configuration { get; }

		protected IFluentProvider Provider =>
			Configuration.Services.GetService<IFluentProvider>();

		protected Timestamped<FluentResource> LastResult { get; set; }

		public CultureInfo DefaultCulture { get; set; }

		public FluentClient(IFluentConfiguration configuration)
		{
			Configuration = configuration;
		}

		protected virtual CultureInfo GetCulture(CultureInfo culture = null)
		{
			return culture ?? DefaultCulture ?? CultureInfo.CurrentCulture;
		}

		public async Task<string> Translate(string message, IDictionary<string, object> parameters = null, CultureInfo culture = null)
		{
			culture = GetCulture(culture);

			var next = await Provider.GetResourceAsync(culture);
			if (next?.Value is null)
				return message;

			await Semaphore.WaitAsync();
			try
			{
				var lastResult = LastResult;
				
			}
			finally
			{
				Semaphore.Release();
			}

			return message;
		}
	}
}