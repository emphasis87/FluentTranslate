using System.Collections;
using FluentTranslate.Domain;

namespace FluentTranslate.Parser
{
    public class FluentEmptyLines : IFluentEntry, IFluentAggregable
	{
        public string Type { get; } = "empty";

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
