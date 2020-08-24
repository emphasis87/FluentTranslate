using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FtlVariant
	{
		public bool IsDefault { get; set; }
		public IFtlVariantKey Key { get; set; }
		public List<IFtlExpression> Values { get; set; } = new List<IFtlExpression>();
	}
}