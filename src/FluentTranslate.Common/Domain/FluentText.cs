using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public class FluentText : IFluentContent
	{
		public string Value { get; set; }

		public static FluentText Aggregate(FluentText left, FluentText right)
		{
			return new FluentText
			{
				Value = $"{left.Value}{right.Value}"
			};
		}

		public bool Equals(object other, IEqualityComparer comparer)
		{
			if (ReferenceEquals(other, this)) return true;
			if (other is null) return false;
			if (!(other is FluentText text)) return false;
			return comparer.Equals(Value, text.Value);
		}

		public int GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(Value);
		}
	}
}