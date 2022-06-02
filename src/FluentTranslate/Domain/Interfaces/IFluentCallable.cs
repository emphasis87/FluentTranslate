using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public interface IFluentCallable : IFluentElement, IEnumerable<FluentCallArgument>
	{
		string TargetId { get; }

		List<FluentCallArgument> Arguments { get; }
		void Add(FluentCallArgument argument);
	}
}