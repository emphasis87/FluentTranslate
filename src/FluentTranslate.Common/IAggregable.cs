namespace FluentTranslate.Common
{
	public interface IAggregable
	{
		bool CanAggregate(object other);
		object Aggregate(object other);
	}
}