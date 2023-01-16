using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentComment : FluentElement, IFluentResourceEntry
	{
        public int Level { get; set; }
		public string Value { get; set; }

		public FluentComment()
		{

		}

		public FluentComment(int level, string comment) : this()
		{
			Level = level;
			Value = comment;
		}

		public FluentComment(string comment) : this(1, comment)
		{

		}
	}
}