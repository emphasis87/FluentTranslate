using FluentTranslate.Domain;
using FluentTranslate.Services;

namespace FluentTranslate.Infrastructure
{
	public class FluentStaticResourceProvider : IFluentResourceProvider
	{
		protected IFluentConfiguration Configuration { get; }

		protected IFluentCloneFactory CloneFactory =>
			Configuration?.Services.GetService<IFluentCloneFactory>() ?? FluentCloneFactory.Default;

		private readonly FluentDocument _resource;

		public FluentStaticResourceProvider(FluentDocument resource, IFluentConfiguration configuration = null)
		{
			Configuration = configuration;

			_resource = CloneFactory.Clone(resource);
		}

		public Task<FluentDocument> GetResourceAsync(CultureInfo culture = null)
		{
			return Task.FromResult(_resource);
		}
	}
}