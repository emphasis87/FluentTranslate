namespace FluentTranslate.Common.Domain
{
	public class FluentMessageReference : FluentRecordReference
	{
		public override string Reference => $"{Id}{(AttributeId != null ? $".{AttributeId}" : null)}";
	}
}
