using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public abstract class FluentRecordReference : IFluentExpression, IFluentReference
	{
        public abstract string Type { get; }
        public string Id { get; set; }
		public string AttributeId { get; set; }
		public abstract string Reference { get; }

        public abstract bool Equals(object other, IEqualityComparer comparer);
        public abstract int GetHashCode(IEqualityComparer comparer);
    }
}