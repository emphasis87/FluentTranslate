using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace FluentTranslate.Common.Domain
{
	public class FluentFunctionCall : IFluentExpression, IFluentCallable
	{
		public string Id { get; set; }
		public IList<FluentCallArgument> Arguments { get; set; }

		public FluentFunctionCall()
		{
			Arguments = new List<FluentCallArgument>();
		}

        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentFunctionCall functionCall)) return false;
            return Id == functionCall.Id &&
                comparer.Equals(Arguments, functionCall.Arguments);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return RuntimeHelpers.GetHashCode(Id);
        }
    }
}