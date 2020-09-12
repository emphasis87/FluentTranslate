using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FluentTranslate.Common.Domain
{
	public class FluentSelection : IFluentExpression, IEnumerable<FluentVariant>
    {
        public string Type { get; } = "selection";
        public IFluentExpression Match { get; set; }
		public IList<FluentVariant> Variants { get; set; }

		public FluentSelection()
		{
			Variants = new List<FluentVariant>();
		}

		public FluentSelection(IFluentExpression match) : this()
		{
			Match = match;
		}

		public bool Equals(object other, IEqualityComparer comparer)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other is null) return false;
			if (!(other is FluentSelection selection)) return false;
			return comparer.Equals(Match, selection.Match)
				&& comparer.Equals(Variants?.ToImmutableArray(), selection.Variants?.ToImmutableArray());
		}

		public int GetHashCode(IEqualityComparer comparer)
		{
			unchecked
			{
				return 
					comparer.GetHashCode(Match) * 397 ^ 
					comparer.GetHashCode(Variants);
			}
		}

		public IEnumerator<FluentVariant> GetEnumerator()
		{
			return Variants.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(FluentVariant variant)
		{
			Variants.Add(variant);
		}
	}
}