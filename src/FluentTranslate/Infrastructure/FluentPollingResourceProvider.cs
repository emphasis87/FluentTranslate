using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public abstract class FluentPollingResourceProvider : IFluentResourceProvider
	{
		protected IFluentConfiguration Configuration { get; }

		protected IFluentEqualityComparer EqualityComparer =>
			Configuration?.Services.GetService<IFluentEqualityComparer>() ?? FluentEqualityComparer.Default;

		private readonly ConcurrentDictionary<CultureInfo, Context> _contextByCulture =
			new ConcurrentDictionary<CultureInfo, Context>();

		/// <summary>
		/// Sets or gets whether the provider should create caching context for each requested culture.
		/// </summary>
		public bool IsLocalized { get; set; }

		/// <summary>
		/// Sets or gets whether the provider should periodically poll the underlying resource for changes.
		/// </summary>
		public bool IsPollingEnabled { get; set; } = true;

		protected ICurrentCultureProvider CultureProvider =>
			Configuration?.Services.GetService<ICurrentCultureProvider>() ?? CurrentCultureProvider.Default;
		
		protected FluentPollingResourceProvider(IFluentConfiguration configuration = null)
		{
			Configuration = configuration;
		}

		protected virtual TimeSpan GetPollingInterval()
		{
			return Configuration?.Options.Get<FluentResourceProviderOptions>()?.PollingInterval
				?? TimeSpan.FromSeconds(1);
		}

		protected virtual CultureInfo GetCulture(CultureInfo culture = null)
		{
			return IsLocalized 
				? culture ?? CultureProvider?.Culture ?? CultureInfo.InvariantCulture 
				: CultureInfo.InvariantCulture;
		}

		protected virtual Context GetContext(CultureInfo culture)
		{
			if (_contextByCulture.TryGetValue(culture, out var context))
				return context;

			var next = CreateContext(culture);
			var result = _contextByCulture.GetOrAdd(culture, next);
			return result;
		}

		protected virtual IEnumerable<Context> GetContexts()
		{
			return _contextByCulture.Values;
		}

		protected virtual Context CreateContext(CultureInfo culture)
		{
			return new Context(culture);
		}

		public async Task<FluentResource> GetResourceAsync(CultureInfo culture = null)
		{
			var nextCulture = GetCulture(culture);

			var context = GetContext(nextCulture);

			var pollingInterval = GetPollingInterval();
			var now = DateTime.Now;

			var lastResult = context.LastResult;
			if (IsPollingEnabled)
			{
				if (context.LastPolled + pollingInterval > now)
					return lastResult;
			}
			else if (lastResult != null)
			{
				return lastResult;
			}

			if (lastResult is null)
			{
				var initResult = new AsyncLazy<FluentResource>(() => FindResourceAsync(context, culture));
				var prevResult = Interlocked.CompareExchange(ref context.InitialResult, initResult, null);
				var result = await (prevResult ?? initResult) ?? new FluentResource();
				var prev = Interlocked.CompareExchange(ref context.LastResult, result, null);
				context.LastPolled = now;
				context.InitialResult = null;
				return prev ?? result;
			}

			var isUpdating = Interlocked.CompareExchange(ref context.IsUpdating, 1, 0);
			if (isUpdating == 0)
			{
				var result = await FindResourceAsync(context, culture);
				if (result != null && !EqualityComparer.Equals(result, context.LastResult))
				{
					Interlocked.Exchange(ref context.LastResult, result);
				}
				Interlocked.Exchange(ref context.IsUpdating, 0);
				context.LastPolled = now;
				return context.LastResult;
			}

			return context.LastResult;
		}

		/// <summary>
		/// Retrieve the resource from the underlying source.
		/// Return null when the source has not been changed.
		/// Return empty <see cref="FluentResource"/> when the source can not be reached.
		/// </summary>
		protected abstract Task<FluentResource> FindResourceAsync(Context context, CultureInfo culture);

		protected class Context
		{
			public CultureInfo Culture { get; }
			public DateTime? LastPolled;
			public FluentResource LastResult;
			public AsyncLazy<FluentResource> InitialResult;
			public int IsUpdating;

			public Context(CultureInfo culture)
			{
				Culture = culture;
			}
		}
	}
}
