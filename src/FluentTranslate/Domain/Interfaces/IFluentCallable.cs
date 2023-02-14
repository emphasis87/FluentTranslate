namespace FluentTranslate.Domain
{
	public interface IFluentCallable : IFluentElement, IEnumerable<FluentCallArgument>
	{
		string Id { get; }

		List<FluentCallArgument> Arguments { get; }
		void Add(FluentCallArgument argument);
	}
}