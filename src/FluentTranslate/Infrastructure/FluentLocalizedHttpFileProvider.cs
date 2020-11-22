using System.Net.Http;

namespace FluentTranslate.Infrastructure
{
	public class FluentLocalizedHttpFileProvider : FluentLocalizedFileProvider
	{
		protected HttpClient Client { get; }

		public FluentLocalizedHttpFileProvider(string rootPath, HttpClient client, IFluentConfiguration configuration = null)
			: base(rootPath, configuration)
		{
			Client = client;
		}

		protected override IFluentProvider CreateProvider(string path)
		{
			return new FluentHttpFileProvider(path, Client, Configuration);
		}
	}
}