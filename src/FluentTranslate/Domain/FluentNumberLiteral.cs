using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentNumberLiteral : FluentElement, IFluentExpression, IFluentVariantIdentifier
    {
        public string Value { get; set; }

		public FluentNumberLiteral()
		{
		}

		public FluentNumberLiteral(string value) : this()
		{
			Value = value;
		}
	}
}