using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    [Entity(FluentTranslateEntities.StringLiteral)]
    public partial class FluentStringLiteral : FluentElement, IFluentExpression
    {
		public string Value { get; set; } = default!;

		public FluentStringLiteral()
		{
		}

		public FluentStringLiteral(string value) : this()
		{
			Value = value;
		}
    }
}