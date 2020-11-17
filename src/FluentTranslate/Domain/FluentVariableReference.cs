using System;
using System.Collections;

namespace FluentTranslate.Domain
{
	public class FluentVariableReference : FluentElement, IFluentExpression
	{
        public override string Type => "variable-ref";

        public string Id { get; set; }

		public FluentVariableReference()
		{
		}

		public FluentVariableReference(string id) : this()
		{
			Id = id;
		}
	}
}
