using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    public class FluentCallArgument : FluentElement
    {
        public string? Identifier { get; set; }
        public IFluentExpression Content { get; set; } = default!;

        public FluentCallArgument()
        {

        }

        public FluentCallArgument(IFluentExpression content) : this()
        {
            Content = content;
        }

        public FluentCallArgument(string id, IFluentExpression content) : this()
        {
            Identifier = id;
            Content = content;
        }
    }
}