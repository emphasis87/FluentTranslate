using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public interface IFluentCallable : IFluentElement
	{
		string Id { get; }
		List<FluentCallArgument> Arguments { get; }
	}
}