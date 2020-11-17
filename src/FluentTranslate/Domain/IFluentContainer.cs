using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public interface IFluentContainer : IFluentElement
	{
		List<IFluentContent> Content { get; }
	}
}