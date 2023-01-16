using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public interface IFluentFunctionCall : IFluentElement, IFluentExpression, IFluentCallable
	{
		
	}

	public class FluentFunctionCall : FluentElement, IFluentFunctionCall, IEnumerable<FluentCallArgument>
    {
        public string TargetId { get; set; }
		public List<FluentCallArgument> Arguments { get; set; }

		public FluentFunctionCall()
		{
			Arguments = new List<FluentCallArgument>();
		}

		public FluentFunctionCall(string id) : this()
		{
			TargetId = id;
		}

        public void Add(FluentCallArgument argument) => Arguments.Add(argument);
        public IEnumerator<FluentCallArgument> GetEnumerator() => Arguments.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}