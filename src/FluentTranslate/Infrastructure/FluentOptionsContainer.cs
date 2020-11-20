using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentOptionsContainer
	{
		T Get<T>();
		void Add(object options);
		bool Remove(object options);
	}

	public class FluentOptionsContainer : IFluentOptionsContainer
	{
		private readonly List<object> _options =
			new List<object>();

		public T Get<T>()
		{
			return _options.OfType<T>().FirstOrDefault(x => x.GetType() == typeof(T))
				?? _options.OfType<T>().FirstOrDefault()
				?? Activator.CreateInstance<T>();
		}

		public void Add(object options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			if (!_options.Contains(options))
				_options.Add(options);
		}

		public bool Remove(object options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			return _options.Remove(options);
		}
	}
}
