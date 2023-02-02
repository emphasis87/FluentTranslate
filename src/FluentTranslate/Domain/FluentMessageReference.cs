using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentMessageReference : FluentRecordReference
    {
		public override string Target => AttributeId switch
		{
			null => $"{Id}",
			_ => $"{Id}.{AttributeId}"
		};

		public FluentMessageReference()
		{
		}

		public FluentMessageReference(string id ) : this()
		{
			Id = id;
		}
	}
}
