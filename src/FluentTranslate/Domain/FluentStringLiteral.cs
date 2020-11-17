using System.Collections;

namespace FluentTranslate.Domain
{
	public class FluentStringLiteral : FluentElement, IFluentExpression
    {
        public override string Type => "string";

        public string Value { get; set; }

		public FluentStringLiteral()
		{
		}

		public FluentStringLiteral(string value) : this()
		{
			Value = value;
		}
    }
}