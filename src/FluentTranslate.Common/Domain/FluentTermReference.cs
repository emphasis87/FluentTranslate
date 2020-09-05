using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FluentTranslate.Common.Domain
{
	public class FluentTermReference : FluentRecordReference, IFluentCallable
	{
		public IList<FluentCallArgument> Arguments { get; set; }

		public FluentTermReference()
		{
			Arguments = new List<FluentCallArgument>();
		}

		public static FluentTermReference Aggregate(FluentTermReference left, FluentTermReference right)
		{
			return new FluentTermReference
			{
				Id = left.Id ?? right.Id,
				Arguments = left.Arguments.Concat(right.Arguments).ToList(),
			};
		}

        public override string Reference => AttributeId switch
        {
            null => $"-{Id}",
            _ => $"-{Id}.{AttributeId}"
        };

        public override bool Equals(object other, IEqualityComparer comparer)
        {
			if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentTermReference termReference)) return false;
            return comparer.Equals(Reference, termReference.Reference)
				&& comparer.Equals(Arguments?.ToImmutableArray(), termReference.Arguments?.ToImmutableArray());
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Reference);
        }
    }
}