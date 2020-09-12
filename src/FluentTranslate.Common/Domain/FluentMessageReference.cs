using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public class FluentMessageReference : FluentRecordReference
    {
        public override string Type { get; } = "message-ref";

        public FluentMessageReference()
		{
		}

		public FluentMessageReference(string id ) : this()
		{
			Id = id;
		}

		public override string Reference => AttributeId switch
		{
			null => $"{Id}",
			_ => $"{Id}.{AttributeId}"
		};

		public override bool Equals(object other, IEqualityComparer comparer)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other is null) return false;
			if (!(other is FluentMessageReference messageReference)) return false;
			return comparer.Equals(Reference, messageReference.Reference);
		}

		public override int GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(Reference);
		}
	}
}
