namespace FluentTranslate.Common.Domain
{
	public class FtlPlaceable : IFtlExpression, IFtlInlineExpression
	{
		public IFtlPlaceableExpression Content { get; set; }
	}
}
