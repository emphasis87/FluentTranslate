namespace FluentTranslate.Common.Domain
{
	public class FluentCallArgument : IFluentElement
	{
		public string Id { get; set; }
		public IFluentExpression Value { get; set; }
	}
}