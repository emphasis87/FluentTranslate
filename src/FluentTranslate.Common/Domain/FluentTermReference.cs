using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FluentTermReference : IFluentExpression
	{
		public string Id { get; set; }
		public string AttributeId { get; set; }
		public IList<FluentCallArgument> Arguments { get; set; }

		public FluentTermReference()
		{
			Arguments = new List<FluentCallArgument>();
		}

		public string Name => $"-{Id}{(AttributeId != null ? $".{AttributeId}" : null)}";
	}
}