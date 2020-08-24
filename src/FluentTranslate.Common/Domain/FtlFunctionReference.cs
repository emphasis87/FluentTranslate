using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FtlFunctionReference : IFtlInlineExpression
	{
		public FtlIdentifier Identifier { get; set; }
		public List<FtlCallArgument> Arguments { get; set; } = new List<FtlCallArgument>();
	}
}