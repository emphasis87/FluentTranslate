namespace FluentTranslate.Common.Domain
{
	public class FluentMessageReference : IFluentExpression
	{
		public string Id { get; set; }
		public string AttributeId { get; set; }

		public string Name => $"{Id}{(AttributeId != null ? $".{AttributeId}" : null)}";
	}
}
