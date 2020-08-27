using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FtlTerm : IFtlEntry, IFtlContentEntry
	{
		public FtlComment Comment { get; set; }
		public string Name { get; set; }
		public IList<IFtlExpression> Content { get; set; }
		public IList<FtlAttribute> Attributes { get; set; }
	}
}