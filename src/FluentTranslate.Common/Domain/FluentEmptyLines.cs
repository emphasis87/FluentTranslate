namespace FluentTranslate.Common.Domain
{
	public class FluentEmptyLines : IFluentElement, IFluentEntry
	{
		public static FluentEmptyLines Aggregate(FluentEmptyLines left, FluentEmptyLines right) => new FluentEmptyLines();
	}
}