using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentSelection : FluentElement, IFluentExpression, IEnumerable<FluentVariant>
    {
        public IFluentExpression Match { get; set; } = default!;
        public List<FluentVariant> Variants { get; } = new();

		public FluentSelection()
		{
			
		}

		public FluentSelection(IFluentExpression match) : this()
		{
			Match = match;
		}

        public void Add(FluentVariant variant) => Variants.Add(variant);

        public IEnumerator<FluentVariant> GetEnumerator() => Variants.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}