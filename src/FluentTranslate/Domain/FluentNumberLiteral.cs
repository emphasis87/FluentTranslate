using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    [Entity(FluentTranslateEntities.NumberLiteral)]
    public class FluentNumberLiteral : FluentElement, IFluentExpression, IFluentVariantIdentifier
    {
		public string Value { get; set; } = default!;

		public FluentNumberLiteral()
		{
		}

		public FluentNumberLiteral(string value) : this()
		{
			Value = value;
		}
	}
}