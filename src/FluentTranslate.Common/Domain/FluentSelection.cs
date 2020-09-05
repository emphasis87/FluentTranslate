using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FluentTranslate.Common.Domain
{
	public class FluentSelection : IFluentExpression
	{
		public IFluentExpression Match { get; set; }
		public IList<FluentVariant> Variants { get; set; }

		public FluentSelection()
		{
			Variants = new List<FluentVariant>();
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
	}
}