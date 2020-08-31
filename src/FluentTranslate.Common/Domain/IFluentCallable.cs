using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public interface IFluentCallable : IFluentElement
	{
		string Id { get; }
		IList<FluentCallArgument> Arguments { get; }
	}
}