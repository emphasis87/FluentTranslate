using System.Collections;

namespace FluentTranslate.Domain
{
	public class FluentNumberLiteral : FluentElement, IFluentExpression, IFluentVariantKey
    {
        public override string Type => "number";

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