using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public interface IFluentFunctionCall : IFluentElement, IFluentExpression, IFluentCallable
	{
		
	}

	public class FluentFunctionCall : IFluentFunctionCall, IEnumerable<FluentCallArgument>
    {
        public string Type => FluentElementTypes.FunctionCall;
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

		public void Add(FluentCallArgument argument)
		{
			Arguments.Add(argument);
		}

        public IEnumerator<FluentCallArgument> GetEnumerator() => Arguments.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}