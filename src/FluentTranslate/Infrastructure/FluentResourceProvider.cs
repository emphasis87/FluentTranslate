using System.Globalization;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentResourceProvider
	{
		Task<FluentResource> GetResourceAsync(CultureInfo culture = null);
	}
}