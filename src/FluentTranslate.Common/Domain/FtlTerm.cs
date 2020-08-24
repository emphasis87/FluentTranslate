using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FtlTerm : IFtlEntry
	{
		public string Comment { get; set; }
		public string Name { get; set; }
		public List<IFtlExpression> Content { get; set; } = new List<IFtlExpression>();
		public List<FtlAttribute> Attributes { get; set; } = new List<FtlAttribute>();
	}
}