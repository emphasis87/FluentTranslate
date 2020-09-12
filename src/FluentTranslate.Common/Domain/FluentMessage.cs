using System.Collections;
using System.Collections.Immutable;

namespace FluentTranslate.Common.Domain
{
	public class FluentMessage : FluentRecord, IFluentEntry
    {
        public override string Type { get; } = "message";
        public override string Reference => Id;

		public FluentMessage()
		{
		}

		public FluentMessage(string id) : this()
		{
			Id = id;
		}

		public FluentMessage(string id, string comment) : this()
		{
			Id = id;
			Comment = comment;
		}

		public override bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentMessage message)) return false;
            return comparer.Equals(Id, message.Id) 
				&& comparer.Equals(Content?.ToImmutableArray(), message.Content?.ToImmutableArray()) 
				&& comparer.Equals(Attributes?.ToImmutableArray(), message.Attributes?.ToImmutableArray());
        }

        public override int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Id);
        }
    }
}
