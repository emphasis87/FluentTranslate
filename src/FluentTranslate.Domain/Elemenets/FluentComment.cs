using System;
using System.Linq;

namespace FluentTranslate.Domain
{
	public interface IFluentComment : IFluentElement, IFluentEntry
    {
		int Level { get;}
		string Comment { get; }
	}

    public class FluentComment : FluentElement, IFluentComment, IFluentAggregable
	{
        public override string Type => FluentElementTypes.Comment;
        public int Level { get; set; }
		public string Comment { get; set; }

		public FluentComment()
		{

		}

		public FluentComment(int level, string comment) : this()
		{
			Level = level;
			Comment = comment;
		}

		public FluentComment(string comment) : this(1, comment)
		{

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
					Comment = JoinLines(Comment, comment.Comment);
					return this;
				}
				case FluentRecord record when Level == 1:
				{
					record.Comment = JoinLines(Comment, record.Comment);
					return record;
				}
				default:
					throw new ArgumentOutOfRangeException(nameof(other));
			}
		}
	}
}