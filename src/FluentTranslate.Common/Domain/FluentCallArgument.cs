using System.Collections;
using System.Runtime.CompilerServices;

namespace FluentTranslate.Common.Domain
{
	public class FluentCallArgument : IFluentElement
	{
		public string Id { get; set; }
		public IFluentExpression Value { get; set; }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentCallArgument argument)) return false;
            return Id == argument.Id &&
                comparer.Equals(Value, argument.Value);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return RuntimeHelpers.GetHashCode(Id);
        }
    }
}