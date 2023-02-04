using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentMessageReference : FluentRecordReference
    {
		public override string Target => string.Join(".", new[] { Id, AttributeId }.Where(x => x is not null));

		public FluentMessageReference()
		{
		}

		public FluentMessageReference(string id ) : this()
		{
			Id = id;
		}

        public FluentMessageReference(string id, string? attributeId) : this()
        {
            Id = id;
			AttributeId = attributeId;
        }
    }
}
