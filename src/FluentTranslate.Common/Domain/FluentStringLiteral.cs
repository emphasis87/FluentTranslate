using System.Collections;
using System.Runtime.CompilerServices;

namespace FluentTranslate.Common.Domain
{
	public class FluentStringLiteral : IFluentExpression
	{
		public string Value { get; set; }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentStringLiteral stringLiteral)) return false;
            return Value == stringLiteral.Value;
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return RuntimeHelpers.GetHashCode(Value);
        }
    }
}