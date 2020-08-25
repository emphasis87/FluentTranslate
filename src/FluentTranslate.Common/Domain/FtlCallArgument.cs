namespace FluentTranslate.Common.Domain
{
	public class FtlCallArgument : IFtlElement
	{
		public string Name { get; set; }
		public IFtlInlineExpression ExpressionValue { get; set; }
	}
}