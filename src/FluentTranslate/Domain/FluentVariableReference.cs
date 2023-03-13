using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    [Entity(FluentTranslateEntities.VariableReference)]
    public class FluentVariableReference : FluentElement, IFluentExpression, IFluentReference
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
