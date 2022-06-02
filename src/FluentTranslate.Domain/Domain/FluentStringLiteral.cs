namespace FluentTranslate.Domain
{
    public class FluentStringLiteral : FluentElement, IFluentExpression
    {
        public override string Type => FluentElementTypes.StringLiteral;
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