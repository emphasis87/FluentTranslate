using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public interface IFluentContainer
	{
		IList<IFluentContent> Content { get; }
	}
}