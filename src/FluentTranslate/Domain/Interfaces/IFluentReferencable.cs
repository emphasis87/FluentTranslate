namespace FluentTranslate.Domain
{
	public interface IFluentReferencable : IFluentElement
	{
		string Reference { get; }
	}
}
