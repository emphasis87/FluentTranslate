using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FluentTranslate.Infrastructure;

namespace FluentTranslate
{
	public interface IFluentClient
	{
		Task<string> Translate(string message, IDictionary<string, object> parameters = null, CultureInfo culture = null);
	}

	public class FluentClient : IFluentClient
	{
		protected IFluentConfiguration Configuration { get; }

		private readonly ConcurrentDictionary<CultureInfo, Context> _contextByCulture =
			new ConcurrentDictionary<CultureInfo, Context>();

		protected IFluentProvider Provider =>
			Configuration.Services.GetService<IFluentProvider>();

		public CultureInfo DefaultCulture { get; set; }

		public FluentClient(IFluentConfiguration configuration)
		{
			Configuration = configuration;
		}

		protected virtual CultureInfo GetCulture(CultureInfo culture = null)
		{
			return culture ?? DefaultCulture ?? CultureInfo.CurrentCulture;
		}

		protected virtual IFluentEngineCache CreateEngineCache(CultureInfo culture)
		{
			return new FluentEngineCache(Configuration);
		}

		protected virtual Context CreateContext(CultureInfo culture)
		{
			return new Context(CreateEngineCache(culture));
		}

		public async Task<string> Translate(string message, IDictionary<string, object> parameters = null, CultureInfo culture = null)
		{
			culture = GetCulture(culture);

			if (!_contextByCulture.TryGetValue(culture, out var context))
			{
				context = CreateContext(culture);
				context = _contextByCulture.GetOrAdd(culture, context);
			}

			var engineCache = context.Cache;
			var resource = await Provider.GetResourceAsync(culture);
			var engine = engineCache.GetEngine(resource);
			var result = engine.Evaluate(message, parameters);
			return result;
		}

		protected class Context
		{
			public IFluentEngineCache Cache { get; }

			public Context(IFluentEngineCache cache)
			{
				Cache = cache;
			}
		}
	}
}