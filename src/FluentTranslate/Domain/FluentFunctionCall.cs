using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public interface IFluentFunctionCall : IFluentElement, IFluentExpression, IFluentCallable
	{
		
	}

    [Entity(FluentTranslateEntities.FunctionCall)]
    public class FluentFunctionCall : FluentElement, IFluentFunctionCall, 
		IEnumerable<FluentCallArgument>
    {
		public string Id { get; set; } = default!;

		public List<FluentCallArgument> Arguments { get; init; } = new();

		public FluentFunctionCall()
		{
			
		}

		public FluentFunctionCall(string id) : this()
		{
			Id = id;
		}

        public void Add(FluentCallArgument argument) => Arguments.Add(argument);
        public IEnumerator<FluentCallArgument> GetEnumerator() => Arguments.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}