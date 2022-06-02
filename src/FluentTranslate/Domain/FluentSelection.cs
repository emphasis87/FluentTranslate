using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentTranslate.Domain
{
	public class FluentSelection : FluentElement, IFluentExpression, IEnumerable<FluentVariant>
    {
        public override string Type => FluentElementTypes.Selection;
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

        public void Add(FluentVariant variant)
		{
			Variants.Add(variant);
		}

		public IEnumerator<FluentVariant> GetEnumerator() => Variants.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}