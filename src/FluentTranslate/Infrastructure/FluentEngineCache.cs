using System;
using System.Collections.Generic;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentEngineCache
	{
		IFluentEngine GetEngine(FluentResource resource);
	}

	public class FluentEngineCache : IFluentEngineCache
	{
		protected IFluentConfiguration Configuration { get; }

		private readonly object _lock = new object();
		private readonly FluentResource _defaultResource = new FluentResource();
		private readonly Dictionary<FluentResource, Context> _engineByResource = new Dictionary<FluentResource, Context>();
		private DateTime _lastEviction;
		
		public FluentEngineCache(IFluentConfiguration configuration)
		{
			Configuration = configuration;
		}

		protected virtual IFluentEngine CreateEngine(FluentResource resource)
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

		public IFluentEngine GetEngine(FluentResource resource)
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
			public FluentResource Resource { get; set; }
			public IFluentEngine Engine { get; set; }
			public DateTime LastAccess { get; set; }
		}
	}

	
}
