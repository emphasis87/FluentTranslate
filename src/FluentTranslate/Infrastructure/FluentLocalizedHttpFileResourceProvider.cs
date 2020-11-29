using System.Net.Http;

namespace FluentTranslate.Infrastructure
{
	public class FluentLocalizedHttpFileResourceProvider : FluentLocalizedFileResourceProvider
	{
		protected HttpClient Client { get; }

		public FluentLocalizedHttpFileResourceProvider(string rootPath, HttpClient client, IFluentConfiguration configuration = null)
			: base(rootPath, configuration)
		{
			Client = client;
		}

		protected override IFluentResourceProvider CreateProvider(string path)
		{
			return new FluentHttpFileResourceProvider(path, Client, Configuration);
		}
	}
}