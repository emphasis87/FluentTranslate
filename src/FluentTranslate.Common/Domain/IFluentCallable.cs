using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public interface IFluentCallable
	{
		string Id { get; }
		IList<FluentCallArgument> Arguments { get; }
	}
}