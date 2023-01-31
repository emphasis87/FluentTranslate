using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentResourceProvider
	{
		Task<FluentDocument> GetResourceAsync(CultureInfo culture = null);
	}
}