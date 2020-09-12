using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public class FluentStringLiteral : IFluentExpression
    {
        public string Type { get; } = "string";
        public string Value { get; set; }

		public FluentStringLiteral()
		{
		}

		public FluentStringLiteral(string value) : this()
		{
			Value = value;
		}

		public bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentStringLiteral stringLiteral)) return false;
            return Value == stringLiteral.Value;
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Value);
        }
    }
}