using System;
using System.Collections;
using System.Linq;

namespace FluentTranslate.Common.Domain
{
	public class FluentComment : IFluentEntry, IAggregable
    {
        public string Type { get; } = "comment";
        public int Level { get; set; }
		public string Value { get; set; }

		public FluentComment()
		{
		}

		public FluentComment(string value) : this()
		{
			Level = 1;
			Value = value;
		}

		public FluentComment(int level, string value) : this()
		{
			Level = level;
			Value = value;
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

		public bool CanAggregate(object other)
		{
			if (ReferenceEquals(this, other)) return false;
			return other switch
			{
				null => false,
				FluentComment comment when comment.Level == Level => true,
				FluentRecord _ when Level == 1 => true,
				_ => false
			};
		}

		public object Aggregate(object other)
		{
			static string JoinLines(string a, string b) => string.Join("\r\n", new[] {a, b}.Where(x => x != null));

			switch(other)
			{
				case FluentComment comment when comment.Level == Level:
				{
					Value = JoinLines(Value, comment.Value);
					return this;
				}
				case FluentRecord record when Level == 1:
				{
					record.Comment = JoinLines(Value, record.Comment);
					return record;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(other));
			}
		}
	}
}