using System.Collections;

namespace FluentTranslate.Domain
{
	public class FluentMessageReference : FluentRecordReference
    {
        public override string Type => "message-ref";

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
	}
}
