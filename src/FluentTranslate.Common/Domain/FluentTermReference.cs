using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;

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
			return new FluentTermReference()
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
            return Reference == termReference.Reference &&
                comparer.Equals(Arguments, termReference.Arguments);
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            return RuntimeHelpers.GetHashCode(Reference);
        }
    }
}