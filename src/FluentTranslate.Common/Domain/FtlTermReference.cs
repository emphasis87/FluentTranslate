using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FtlTermReference : IFtlInlineExpression
	{
		public string Identifier { get; set; }
		public string Attribute { get; set; }
		public List<FtlCallArgument> Arguments { get; set; } = new List<FtlCallArgument>();
	}
}