using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentMessageReference : FluentRecordReference
    {
		public override string TargetReference => TargetAttributeId switch
		{
			null => $"{TargetId}",
			_ => $"{TargetId}.{TargetAttributeId}"
		};

		public FluentMessageReference()
		{
		}

		public FluentMessageReference(string id ) : this()
		{
			TargetId = id;
		}
	}
}
