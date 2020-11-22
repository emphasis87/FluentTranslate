using System;
using System.Collections.Generic;
using System.Linq;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentMerger
	{
		FluentResource Combine(IEnumerable<FluentResource> resources);
	}

	/// <summary>
	/// Combines multiple resources into one.
	/// Omits <see cref="FluentComment"/> entries.
	/// </summary>
	public class FluentMerger : IFluentMerger
	{
		public static FluentMerger Default { get; } = new FluentMerger();

		protected IFluentConfiguration Configuration { get; }

		protected IFluentCloneFactory Factory =>
			Configuration?.Services.GetService<IFluentCloneFactory>() ?? FluentCloneFactory.Default;

		public FluentMerger(IFluentConfiguration configuration = null)
		{
			Configuration = configuration;
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

				var factory = Factory ?? FluentCloneFactory.Default;
				var clone = (IFluentEntry)factory.Clone(entry);
				result.Entries.Add(clone);
			}

			return result;
		}
	}
}
