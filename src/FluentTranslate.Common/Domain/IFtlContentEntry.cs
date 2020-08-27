using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public interface IFtlContentEntry
	{
		FtlComment Comment { get; set; }
		string Name { get; set; }
		IList<IFtlExpression> Content { get; set; }
		IList<FtlAttribute> Attributes { get; set; }
	}
}