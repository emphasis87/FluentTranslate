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
	public class FluentCombinator : IFluentCombinator
	{
		public static FluentCombinator Default { get; } = new FluentCombinator();

		private readonly IFluentFactory _factory;

		public FluentCombinator(IFluentFactory factory = null)
		{
			_factory = factory ?? FluentFactory.Default;
		}

		public FluentResource Combine(IEnumerable<FluentResource> resources)
		{
			if (resources is null)
				throw new ArgumentNullException(nameof(resources));

			var result = new FluentResource();
			var names = new HashSet<string>();

			var entries = resources
				.Where(x => x?.Entries != null)
				.SelectMany(x => x.Entries)
				.OfType<FluentRecord>();

			foreach (var entry in entries)
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
