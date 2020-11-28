using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FluentTranslate.Infrastructure;

namespace FluentTranslate
{
	public interface IFluentClient
	{
		string Translate(string query, IDictionary<string, object> parameters = null, CultureInfo culture = null);
		Task<string> TranslateAsync(string query, IDictionary<string, object> parameters = null, CultureInfo culture = null);
	}

	public class FluentClient : IFluentClient
	{
		protected IFluentConfiguration Configuration { get; }

		private readonly ConcurrentDictionary<CultureInfo, Context> _contextByCulture =
			new ConcurrentDictionary<CultureInfo, Context>();

		protected IFluentResourceProvider Provider =>
			Configuration.Services.GetService<IFluentResourceProvider>();

		protected ICurrentCultureProvider CultureProvider =>
			Configuration.Services.GetService<ICurrentCultureProvider>();

		public FluentClient(IFluentConfiguration configuration)
		{
			Configuration = configuration;
		}

		protected virtual CultureInfo GetCulture(CultureInfo culture = null)
		{
			return culture ?? CultureProvider?.Culture ?? CultureInfo.CurrentUICulture;
		}

		protected virtual IFluentEngineCache CreateEngineCache(CultureInfo culture)
		{
			return new FluentEngineCache(Configuration);
		}

		protected virtual Context CreateContext(CultureInfo culture)
		{
			return new Context(CreateEngineCache(culture));
		}

		public string Translate(string query, IDictionary<string, object> parameters = null, CultureInfo culture = null)
		{
			var task = Task.Run(() => TranslateAsync(query, parameters, culture));
			var result = task.Result;
			return result;
		}

		public async Task<string> TranslateAsync(string query, IDictionary<string, object> parameters = null, CultureInfo culture = null)
		{
			culture = GetCulture(culture);

			var context = GetContext(culture);

			var engineCache = context.Cache;
			var resource = await Provider.GetResourceAsync(culture);
			var engine = engineCache.GetEngine(resource);
			
			try
			{
				var result = engine.Evaluate(query, parameters);
				return result;
			}
			catch (Exception)
			{
				return query;
			}
		}

		protected virtual Context GetContext(CultureInfo culture)
		{
			if (!_contextByCulture.TryGetValue(culture, out var context))
			{
				context = CreateContext(culture);
				context = _contextByCulture.GetOrAdd(culture, context);
			}

			return context;
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