using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FluentTranslate.Common.Domain
{
	public class FluentVariant : IFluentContainer
	{
		public bool IsDefault { get; set; }
		public IFluentVariantKey Key { get; set; }
		public IList<IFluentContent> Content { get; set; }

		public FluentVariant()
		{
			Content = new List<IFluentContent>();
		}

		public bool Equals(object other, IEqualityComparer comparer)
		{
			if (ReferenceEquals(other, this)) return true;
			if (other is null) return false;
			if (!(other is FluentVariant variant)) return false;
			return comparer.Equals(IsDefault, variant.IsDefault)
				&& comparer.Equals(Key, variant.Key)
				&& comparer.Equals(Content?.ToImmutableArray(), variant.Content?.ToImmutableArray());
		}

		public int GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(Key);
		}
	}
}