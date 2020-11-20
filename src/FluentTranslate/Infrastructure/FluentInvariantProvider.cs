using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentProvider
	{
		Task<Timestamped<FluentResource>> GetResourceAsync(CultureInfo culture = null);
	}

	public abstract class FluentInvariantProvider : IFluentProvider
	{
		protected SemaphoreSlim Semaphore { get; } = new SemaphoreSlim(1, 1);

		protected IFluentConfiguration Configuration { get; }

		protected IFluentEqualityComparer EqualityComparer =>
			Configuration.Services.GetService<IFluentEqualityComparer>() ?? FluentEqualityComparer.Default;

		private DateTime _lastPolled;
		private Timestamped<FluentResource> _lastResult;

		protected FluentInvariantProvider(IFluentConfiguration configuration)
		{
			Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}

		protected virtual TimeSpan GetPollingInterval()
		{
			var options = Configuration.Options.Get<FluentProviderOptions>();
			var pollingInterval = options.PollingInterval ?? TimeSpan.FromSeconds(5);
			return pollingInterval;
		}

		protected virtual CultureInfo GetCulture(CultureInfo culture = null)
		{
			return CultureInfo.InvariantCulture;
		}

		protected virtual bool GetLastPolled(out DateTime lastPolled, CultureInfo culture = null)
		{
			lastPolled = _lastPolled;
			return _lastResult != null;
		}

		protected virtual void SetLastPolled(DateTime lastPolled, CultureInfo culture = null)
		{
			_lastPolled = lastPolled;
		}

		protected virtual bool GetLastResult(out Timestamped<FluentResource> lastResult, CultureInfo culture = null)
		{
			lastResult = _lastResult;
			return _lastResult != null;
		}

		protected virtual void SetLastResult(Timestamped<FluentResource> lastResult, CultureInfo culture = null)
		{
			_lastResult = lastResult;
		}

		public async Task<Timestamped<FluentResource>> GetResourceAsync(CultureInfo culture = null)
		{
			culture = GetCulture(culture);

			var pollingInterval = GetPollingInterval();
			
			await Semaphore.WaitAsync();
			try
			{
				var now = DateTime.Now;

				var hasResult = GetLastResult(out var lastResult, culture);
				var hasPolled = GetLastPolled(out var lastPolled, culture);
				if (hasPolled && lastPolled + pollingInterval > now)
				{
					if (hasResult)
						return lastResult;
				}

				SetLastPolled(now, culture);

				// Find the next resource
				var next = await FindResourceAsync(now, lastResult, culture);

				if (!hasResult)
				{
					next ??= new Timestamped<FluentResource>(now, null);
					SetLastResult(next, culture);
					return next;
				}

				var nextResource = next?.Value;
				if (nextResource is null)
				{
					if (lastResult.Value != null)
					{
						next = new Timestamped<FluentResource>(now, null);
						SetLastResult(next, culture);
					}

					return lastResult;
				}

				var nextTimestamp = next.Timestamp;
				if (nextTimestamp <= lastResult.Timestamp)
					return lastResult;

				var currentResource = lastResult.Value;
				if (EqualityComparer.Equals(currentResource, nextResource))
					return lastResult;

				next = new Timestamped<FluentResource>(nextTimestamp, nextResource);
				SetLastResult(next, culture);
				return next;
			}
			finally
			{
				Semaphore.Release();
			}
		}

		protected abstract Task<Timestamped<FluentResource>> FindResourceAsync(DateTime now, Timestamped<FluentResource> lastResult, CultureInfo culture);

	}

	
}
