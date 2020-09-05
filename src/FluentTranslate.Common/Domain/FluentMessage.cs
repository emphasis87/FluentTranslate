using System.Collections;
using System.Runtime.CompilerServices;

namespace FluentTranslate.Common.Domain
{
	public class FluentMessage : FluentRecord, IFluentEntry
	{
        public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentMessage message)) return false;
            return Id == message.Id &&
                comparer.Equals(Content, message.Content) &&
                comparer.Equals(Attributes, message.Attributes);
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            return RuntimeHelpers.GetHashCode(Id);
        }
    }
}
