using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public class FluentNumberLiteral : IFluentExpression, IFluentVariantKey
    {
        public string Type => "number";
        public string Value { get; set; }

		public FluentNumberLiteral()
		{
		}

		public FluentNumberLiteral(string value) : this()
		{
			Value = value;
		}

		public bool Equals(object other, IEqualityComparer comparer)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other is null) return false;
			if (!(other is FluentNumberLiteral numberLiteral)) return false;
			return comparer.Equals(Value, numberLiteral.Value);
		}

		public int GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(Value);
		}
	}
}