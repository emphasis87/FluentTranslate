using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public class FluentFunctionCall : FluentElement, IFluentExpression, IFluentCallable, IEnumerable<FluentCallArgument>
    {
        public override string Type => FluentElementTypes.FunctionCall;

        public string Id { get; set; }
		public List<FluentCallArgument> Arguments { get; set; }

		public FluentFunctionCall()
		{
			Arguments = new List<FluentCallArgument>();
		}

		public FluentFunctionCall(string id) : this()
		{
			Id = id;
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