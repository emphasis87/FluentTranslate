using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FtlSelectExpression : IFtlPlaceableExpression
	{
		public IFtlInlineExpression Selector { get; set; }
		public List<FtlVariant> Variants { get; set; } = new List<FtlVariant>();
	}
}
