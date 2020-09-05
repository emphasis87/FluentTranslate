using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public class FluentComment : IFluentEntry
	{
		public int Level { get; set; }
		public string Value { get; set; }

		public static bool CanAggregate(FluentComment left, FluentComment right)
		{
			return left.Level == right.Level;
		}

		public static FluentComment Aggregate(FluentComment left, FluentComment right)
		{
			return new FluentComment
			{
				Level = left.Level,
				Value = $"{left.Value}\r\n{right.Value}",
			};
		}

        public bool Equals(object other, IEqualityComparer comparer)
        {
			if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentComment comment)) return false;
            return comparer.Equals(Level, comment.Level) 
				&& comparer.Equals(Value, comment.Value);
		}

        public int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Value);
        }
    }
}