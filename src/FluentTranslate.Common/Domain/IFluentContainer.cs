using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public interface IFluentContainer : IFluentElement
	{
		IList<IFluentContent> Content { get; }
	}
}