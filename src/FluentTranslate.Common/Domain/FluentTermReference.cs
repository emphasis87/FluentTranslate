using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FluentTermReference : FluentRecordReference, IFluentCallable
	{
		public IList<FluentCallArgument> Arguments { get; set; }

		public FluentTermReference()
		{
			Arguments = new List<FluentCallArgument>();
		}

		public override string Reference => $"-{Id}{(AttributeId != null ? $".{AttributeId}" : null)}";
	}
}