using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public interface IFluentContainer : IFluentElement, IEnumerable<IFluentContent>
	{
		List<IFluentContent> Content { get; }
		void Add(IFluentContent content);
	}
}