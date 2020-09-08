using System.Collections;
using System.Collections.Immutable;

namespace FluentTranslate.Common.Domain
{
	public class FluentTerm : FluentRecord, IFluentEntry, IFluentContainer, IFluentReference
	{
		public FluentTerm()
		{
		}

		public FluentTerm(string id) : this()
		{
			Id = id;
		}

		public FluentTerm(string id, string comment) : this()
		{
			Id = id;
			Comment = comment;
		}

		public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentTerm term)) return false;
            return Id == term.Id 
				&& comparer.Equals(Content?.ToImmutableArray(), term.Content?.ToImmutableArray()) 
				&& comparer.Equals(Attributes?.ToImmutableArray(), term.Attributes?.ToImmutableArray());
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Id);
        }

		public override string Reference => $"-{Id}";
	}
}