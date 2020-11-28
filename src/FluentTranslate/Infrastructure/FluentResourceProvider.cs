using System.Globalization;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentResourceProvider
	{
		Task<FluentResource> GetResourceAsync(CultureInfo culture = null);
	}

	public class FluentResourceProvider : IFluentResourceProvider
	{
		protected IFluentConfiguration Configuration { get; }

		protected IFluentCloneFactory CloneFactory =>
			Configuration?.Services.GetService<IFluentCloneFactory>() ?? FluentCloneFactory.Default;

		private readonly FluentResource _resource;

		public FluentResourceProvider(FluentResource resource, IFluentConfiguration configuration = null)
		{
			Configuration = configuration;

			_resource = CloneFactory.Clone(resource);
		}

		public Task<FluentResource> GetResourceAsync(CultureInfo culture = null)
		{
			return Task.FromResult(_resource);
		}
	}
}