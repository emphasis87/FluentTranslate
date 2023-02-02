using FluentTranslate.Domain;
using FluentTranslate.Domain.Common;
using FluentTranslate.Services;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentMerger
	{
		FluentDocument Combine(IEnumerable<FluentDocument> resources);
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

		public FluentDocument Combine(IEnumerable<FluentDocument> resources)
		{
			if (resources is null)
				throw new ArgumentNullException(nameof(resources));

			var result = new FluentDocument();
			/*
			var entryByName = new Dictionary<string, FluentRecord>();

			var entries = resources
				.Where(x => x?.Content != null)
				.SelectMany(x => x.Content)
				.OfType<FluentRecord>();

			foreach (var entry in entries)
			{
				if (entryByName.TryGetValue(entry.Reference, out var prev))
				{
					// Coalesce L1 comments
					prev.Comment ??= entry.Comment;
					continue;
				}

				var factory = Factory ?? FluentCloneFactory.Default;
				var clone = factory.Clone(entry);
				result.Content.Add(clone);
				entryByName[entry.Reference] = clone;
			}
			*/
			return result;
		}
	}
}
