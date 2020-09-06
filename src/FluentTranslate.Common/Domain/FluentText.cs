using System;
using System.Collections;
using System.Linq;

namespace FluentTranslate.Common.Domain
{
	public class FluentText : IFluentContent, IAggregable
	{
		public string Value { get; set; }

		public FluentText()
		{
		}

		public FluentText(string value) : this()
		{
			Value = value;
		}

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

		public bool CanAggregate(object other)
		{
			if (ReferenceEquals(this, other)) return false;
			if (other is null) return false;
			return other is FluentText;
		}

		public object Aggregate(object other)
		{
			static string JoinText(string a, string b) => string.Join("", new[] { a, b }.Where(x => x != null));

			switch (other)
			{
				case FluentText text:
				{
					Value = JoinText(Value, text.Value);
					return this;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(other));
			}
		}
	}
}