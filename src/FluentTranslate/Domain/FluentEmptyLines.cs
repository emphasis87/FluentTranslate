using System.Collections;
using FluentTranslate.Domain;

namespace FluentTranslate.Parser
{
    public class FluentEmptyLines : FluentElement, IFluentEntry, IFluentAggregable
	{
		public override string Type => FluentElementTypes.EmptyLines;
		public int Count { get; set; }

        public bool Equals(object other, IEqualityComparer comparer)
		{
			return true;
		}

		public int GetHashCode(IEqualityComparer comparer)
		{
			return 0;
		}

		public bool CanAggregate(object other)
		{
			if (ReferenceEquals(this, other)) return false;
			if (other is null) return false;
			return other is FluentEmptyLines;
		}

		public object Aggregate(object other)
		{
			return this;
		}
	}
}
