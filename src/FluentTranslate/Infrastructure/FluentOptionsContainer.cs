using System;
using System.Collections.Concurrent;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentOptionsContainer
	{
		T Get<T>();
		void Add<T>(T options);
		bool Remove<T>(T options);
	}

	public class FluentOptionsContainer : IFluentOptionsContainer
	{
		private readonly ConcurrentDictionary<Type, object> _options =
			new ConcurrentDictionary<Type, object>();

		public T Get<T>()
		{
			var type = typeof(T);
			if (_options.TryGetValue(type, out var options))
				return (T) options;

			options = Activator.CreateInstance<T>();
			var result = _options.GetOrAdd(type, options);
			return (T) result;
		}

		public void Add<T>(T options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			var type = typeof(T);
			_options[type] = options;
		}

		public bool Remove<T>(T options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			var type = typeof(T);
			return _options.TryRemove(type, out _);
		}
	}
}
