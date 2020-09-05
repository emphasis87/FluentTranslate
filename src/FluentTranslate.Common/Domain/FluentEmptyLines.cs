using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public class FluentEmptyLines : IFluentElement, IFluentEntry
	{
		public static FluentEmptyLines Aggregate(FluentEmptyLines left, FluentEmptyLines right) => new FluentEmptyLines();
        
        public bool Equals(object other, IEqualityComparer comparer)
        {
            return true;
        }

        public int GetHashCode(IEqualityComparer comparer)
		{
			return 0;
		}
    }
}