using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FluentTranslate.Common.Domain
{
	public class FluentFunctionCall : IFluentExpression, IFluentCallable, IEnumerable<FluentCallArgument>
	{
		public string Id { get; set; }
		public IList<FluentCallArgument> Arguments { get; set; }

		public FluentFunctionCall()
		{
			Arguments = new List<FluentCallArgument>();
		}

		public FluentFunctionCall(string id) : this()
		{
			Id = id;
		}

		public bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentFunctionCall functionCall)) return false;
            return comparer.Equals(Id, functionCall.Id)
				&& comparer.Equals(Arguments?.ToImmutableArray(), functionCall.Arguments?.ToImmutableArray());
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Id);
        }

		public IEnumerator<FluentCallArgument> GetEnumerator()
		{
			return Arguments.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(FluentCallArgument argument)
		{
			Arguments.Add(argument);
		}
	}
}