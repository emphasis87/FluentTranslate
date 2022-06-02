using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{

	public class FluentCallArgument : FluentElement
    {
        public string Identifier { get; set; }
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
			Identifier = id;
			Value = value;
		}
    }
}