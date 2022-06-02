using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentPlaceable : FluentElement, IFluentContent, IFluentExpression
    {
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
