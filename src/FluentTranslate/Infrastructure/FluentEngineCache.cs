using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentEngineCache
	{
		IFluentEngine GetEngine(FluentDocument resource);
	}

	public class FluentEngineCache : IFluentEngineCache
	{
		protected IFluentConfiguration Configuration { get; }

		private readonly object _lock = new object();
		private readonly FluentDocument _defaultResource = new FluentDocument();
		private readonly Dictionary<FluentDocument, Context> _engineByResource = new Dictionary<FluentDocument, Context>();
		private DateTime _lastEviction;
		
		public FluentEngineCache(IFluentConfiguration configuration)
		{
			Configuration = configuration;
		}

		protected virtual IFluentEngine CreateEngine(FluentDocument resource)
		{
			return new FluentEngine(resource, Configuration);
		}

		protected virtual TimeSpan GetEvictionInterval()
		{
			return TimeSpan.FromMinutes(1);
		}

		protected virtual TimeSpan GetEvictionLastAccessInterval()
		{
			return TimeSpan.FromMinutes(15);
		}

		public IFluentEngine GetEngine(FluentDocument resource)
		{
			resource ??= _defaultResource;

			lock (_lock)
			{
				var now = DateTime.Now;

				// Every 1m evict resources not used for at least 15m
				if (_engineByResource.Count > 10 && 
					_lastEviction + GetEvictionInterval() > now)
				{
					_lastEviction = now;
					foreach (var entry in _engineByResource.Values)
					{
						if (entry.LastAccess + GetEvictionLastAccessInterval() < now)
							_engineByResource.Remove(entry.Resource);
					}
				}

				if (!_engineByResource.TryGetValue(resource, out var context))
				{
					context = new Context()
					{
						Resource = resource,
						Engine = CreateEngine(resource)
					};
					_engineByResource[resource] = context;
				}

				context.LastAccess = now;
				return context.Engine;
			}
		}

		private class Context
		{
			public FluentDocument Resource { get; set; }
			public IFluentEngine Engine { get; set; }
			public DateTime LastAccess { get; set; }
		}
	}

	
}
