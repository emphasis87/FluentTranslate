using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentTranslate.Domain;

namespace FluentTranslate
{
	public interface IFluentCombinator
	{
		FluentResource Combine(IEnumerable<FluentResource> resources);
	}

	/// <summary>
	/// Combines multiple resources into one.
	/// Omits <see cref="FluentComment"/> entries.
	/// </summary>
	public class FluentCombinator
	{
		private readonly IFluentFactory _factory;

		public FluentCombinator(IFluentFactory factory = null)
		{
			_factory = factory ?? new FluentFactory();
		}

		public FluentResource Combine(IEnumerable<FluentResource> resources)
		{
			if (resources is null)
				throw new ArgumentNullException(nameof(resources));

			var result = new FluentResource();
			var names = new HashSet<string>();
			foreach (var entry in resources.SelectMany(x => x.Entries).OfType<FluentRecord>())
			{
				if (!names.Add(entry.Reference))
					continue;

				var clone = (IFluentEntry)_factory.Clone(entry);
				result.Entries.Add(clone);
			}

			return result;
		}
	}
}
