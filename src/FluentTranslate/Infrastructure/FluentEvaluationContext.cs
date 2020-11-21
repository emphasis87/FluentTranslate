using System.Collections.Generic;

namespace FluentTranslate.Infrastructure
{
	public class FluentEvaluationContext
	{
		public Dictionary<string, object> Parameters { get; }

		public FluentEvaluationContext(IDictionary<string, object> parameters = null)
		{
			Parameters = parameters != null 
				? new Dictionary<string, object>(parameters) 
				: new Dictionary<string, object>();
		}
	}
}