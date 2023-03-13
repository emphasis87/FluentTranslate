using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    [Entity(FluentTranslateEntities.Placeable)]
    public class FluentPlaceable : FluentElement, IFluentContent, IFluentExpression
    {
        public IFluentExpression Content { get; set; } = default!;

        public FluentPlaceable()
		{
		}

		public FluentPlaceable(IFluentExpression content)
		{
			Content = content;
		}
    }
}
