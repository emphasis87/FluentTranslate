using System.Globalization;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public class FluentStaticResourceProvider : IFluentResourceProvider
	{
		protected IFluentConfiguration Configuration { get; }

		protected IFluentCloneFactory CloneFactory =>
			Configuration?.Services.GetService<IFluentCloneFactory>() ?? FluentCloneFactory.Default;

		private readonly FluentResource _resource;

		public FluentStaticResourceProvider(FluentResource resource, IFluentConfiguration configuration = null)
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