using FluentTranslate.Common;

using System;
using System.Linq;

using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    public class FluentText : FluentElement, IFluentContent, IFluentAggregable
    {
        public string Value { get; set; }

		public FluentText()
		{
		}

		public FluentText(string value) : this()
		{
			Value = value;
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