using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentCallArgument : FluentElement
    {
        public string? Identifier { get; set; }
        public IFluentExpression Content { get; set; } = default!;

        public FluentCallArgument(string? id = null, IFluentExpression? content = null)
        {
            Identifier = id;
            Content = content ?? default!;
        }

        public FluentCallArgument(IFluentExpression content)
        {
            Content = content;
        }
    }
}