using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    public class FluentVariableReference : FluentElement, IFluentExpression, IFluentTargetReference
	{
		public string Id { get; set; } = default!;

		public string Target => Id;

        public FluentVariableReference()
		{
		}

		public FluentVariableReference(string id) : this()
		{
			Id = id;
		}
	}
}
