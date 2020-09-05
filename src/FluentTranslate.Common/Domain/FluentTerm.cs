using System.Collections;
using System.Runtime.CompilerServices;

namespace FluentTranslate.Common.Domain
{
	public class FluentTerm : FluentRecord, IFluentEntry, IFluentContainer
	{
        public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentTerm term)) return false;
            return Id == term.Id &&
                comparer.Equals(Content, term.Content) &&
                comparer.Equals(Attributes, term.Attributes);
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            return RuntimeHelpers.GetHashCode(Id);
        }
    }
}