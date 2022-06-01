using System.Collections;

namespace FluentTranslate.Domain
{
	public class FluentPlaceable : FluentElement, IFluentContent, IFluentExpression
    {
        public override string Type => FluentElementTypes.Placeable;

        public IFluentExpression Content { get; set; }

		public FluentPlaceable()
		{
		}

		public FluentPlaceable(IFluentExpression content) : this()
		{
			Content = content;
		}
	}
}
