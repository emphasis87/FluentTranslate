using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public class FluentSelection : FluentElement, IFluentExpression, IEnumerable<FluentVariant>
    {
        public override string Type => "selection";

        public IFluentExpression Match { get; set; }
		public List<FluentVariant> Variants { get; set; }

		public FluentSelection()
		{
			Variants = new List<FluentVariant>();
		}

		public FluentSelection(IFluentExpression match) : this()
		{
			Match = match;
		}

		public IEnumerator<FluentVariant> GetEnumerator()
		{
			return Variants.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(FluentVariant variant)
		{
			Variants.Add(variant);
		}
	}
}