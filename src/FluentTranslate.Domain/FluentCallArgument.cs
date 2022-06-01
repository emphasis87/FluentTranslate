using System.Collections;

namespace FluentTranslate.Domain
{
	public class FluentCallArgument : FluentElement
    {
        public override string Type => FluentElementTypes.CallArgument;

        public string Id { get; set; }
		public IFluentExpression Value { get; set; }

		public FluentCallArgument()
		{
		}

		public FluentCallArgument(IFluentExpression value) : this()
		{
			Value = value;
		}

		public FluentCallArgument(string id, IFluentExpression value) : this()
		{
			Id = id;
			Value = value;
		}
    }
}