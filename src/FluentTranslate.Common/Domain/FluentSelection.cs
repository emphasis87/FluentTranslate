using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FluentSelection : IFluentExpression
	{
		public IFluentExpression Match { get; set; }
		public IList<FluentVariant> Variants { get; set; }

		public FluentSelection()
		{
			Variants = new List<FluentVariant>();
		}
	}
}