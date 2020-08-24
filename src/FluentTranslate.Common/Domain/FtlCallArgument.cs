namespace FluentTranslate.Common.Domain
{
	public class FtlCallArgument
	{
		public string Name { get; set; }
		public IFtlInlineExpression ExpressionValue { get; set; }
	}
}