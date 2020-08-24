using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FtlAttribute
	{
		public string Identifier { get; set; }
		public List<IFtlExpression> Content { get; set; } = new List<IFtlExpression>();
	}
}