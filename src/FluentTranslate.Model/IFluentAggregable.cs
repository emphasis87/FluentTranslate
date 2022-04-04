namespace FluentTranslate.Domain
{
	public interface IFluentAggregable
	{
		bool CanAggregate(object other);
		object Aggregate(object other);
	}
}