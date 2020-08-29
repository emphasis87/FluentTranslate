namespace FluentTranslate.Common.Domain
{
	public class FluentPlaceable : IFluentContent, IFluentExpression
	{
		public IFluentExpression Content { get; set; }
	}
}
