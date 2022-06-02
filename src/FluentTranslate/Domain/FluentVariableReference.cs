using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    public class FluentVariableReference : FluentElement, IFluentExpression, IFluentTargetReference
	{
        public string TargetId { get; set; }
		public string TargetReference => TargetId;

        public FluentVariableReference()
		{
		}

		public FluentVariableReference(string targetId) : this()
		{
			TargetId = targetId;
		}
	}
}
