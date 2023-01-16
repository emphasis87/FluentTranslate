using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{

	public class FluentCallArgument : FluentElement
    {
        public string? Identifier { get; set; }
		public IFluentExpression Content { get; set; }

		public FluentCallArgument(string? id = null)
		{
			Identifier = id;
		}
    }
}