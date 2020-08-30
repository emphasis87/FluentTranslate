namespace FluentTranslate.Common.Domain
{
	public abstract class FluentRecordReference : IFluentExpression, IFluentReference
	{
		public string Id { get; set; }
		public string AttributeId { get; set; }
		public abstract string Reference { get; }
	}
}