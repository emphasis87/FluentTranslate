using System.Collections.Generic;
using System.Linq;

namespace FluentTranslate.Common.Domain
{
	public class FluentTermReference : FluentRecordReference, IFluentCallable
	{
		public IList<FluentCallArgument> Arguments { get; set; }

		public FluentTermReference()
		{
			Arguments = new List<FluentCallArgument>();
		}

		public static FluentTermReference Aggregate(FluentTermReference left, FluentTermReference right)
		{
			return new FluentTermReference()
			{
				Id = left.Id ?? right.Id,
				Arguments = left.Arguments.Concat(right.Arguments).ToList(),
			};
		}

		public override string Reference => $"-{Id}{(AttributeId != null ? $".{AttributeId}" : null)}";
	}
}