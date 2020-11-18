using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using FluentTranslate.Domain;

namespace FluentTranslate
{
	public interface IFluentConfiguration
	{
		IFluentCombinator Combinator { get; }
		T GetOptions<T>();
	}

	public class FluentConfiguration : IFluentConfiguration
	{
		public IFluentCombinator Combinator { get; }

		private readonly List<object> _options =
			new List<object>();

		private DateTime _lastModified;
		private readonly List<IFluentProvider> _providers =
			new List<IFluentProvider>();
		private readonly Dictionary<CultureInfo, (DateTime LastModified, FluentResource Resource)> _resourceByCulture =
			new Dictionary<CultureInfo, (DateTime LastModified, FluentResource Resource)>();

		public FluentConfiguration(IFluentCombinator combinator = null)
		{
			Combinator = combinator ?? FluentCombinator.Default;
			_lastModified = DateTime.Now;
		}

		public T GetOptions<T>()
		{
			lock (_options)
			{
				return _options.OfType<T>().FirstOrDefault(x => x.GetType() == typeof(T))
					?? _options.OfType<T>().FirstOrDefault();
			}
		}

		public void AddOptions(object options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			lock (_options)
			{
				if (!_options.Contains(options))
					_options.Add(options);
			}
		}

		public bool RemoveOptions(object options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			lock (_options)
			{
				return _options.Remove(options);
			}
		}

		public void Add(IFluentProvider provider)
		{
			lock (_providers)
			{
				if (!_providers.Contains(provider))
				{
					_providers.Add(provider);
					_lastModified = DateTime.Now;
					_resourceByCulture.Clear();
				}
			}
		}

		public bool Remove(IFluentProvider provider)
		{
			lock (_providers)
			{
				if (_providers.Remove(provider))
				{
					_lastModified = DateTime.Now;
					_resourceByCulture.Clear();
					return true;
				}

				return false;
			}
		}

		public (DateTime LastModified, FluentResource Resource) GetResource(CultureInfo culture = null)
		{
			culture ??= CultureInfo.InvariantCulture;

			lock (_providers)
			{
				if (_providers.Count == 0)
					return (_lastModified, new FluentResource());

				var timestampedResources = _providers.Select(x => x.GetResource(culture)).ToArray();
				var lastModified = timestampedResources.Max(x => x.LastModified);
				if (_lastModified < lastModified)
				{
					// Providers have updated a resource
					_lastModified = lastModified;
					_resourceByCulture.Remove(culture);
				}
				else if (_resourceByCulture.TryGetValue(culture, out var current) && _lastModified == current.LastModified)
				{
					return (_lastModified, current.Resource);
				}

				// Provider list has been updated
				var resources = timestampedResources.Select(x => x.Resource).ToArray();
				var resource = Combinator.Combine(resources);
				_resourceByCulture[culture] = (_lastModified, resource);

				return (lastModified, resource);
			}
		}
	}
}
