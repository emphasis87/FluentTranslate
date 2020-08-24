namespace FluentTranslate.Common.Domain
{
	public class FtlNumberLiteral : IFtlInlineExpression, IFtlVariantKey
	{
		public string Value { get; set; }
	}
}