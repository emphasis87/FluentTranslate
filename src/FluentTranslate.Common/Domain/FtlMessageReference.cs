namespace FluentTranslate.Common.Domain
{
	public class FtlMessageReference : IFtlInlineExpression
	{
		public string Identifier { get; set; }
		public string Attribute { get; set; }
	}
}
