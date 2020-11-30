using System.Reflection;

namespace FluentTranslate.Infrastructure
{
	public class FluentLocalizedEmbeddedResourceProvider : FluentLocalizedFileResourceProvider
	{
		public Assembly Assembly { get; }

		public FluentLocalizedEmbeddedResourceProvider(Assembly assembly, string rootPath, IFluentConfiguration configuration = null) 
			: base(rootPath, configuration)
		{
			Assembly = assembly;
			IsPollingEnabled = false;
		}

		protected override IFluentResourceProvider CreateProvider(string path)
		{
			return new FluentEmbeddedResourceProvider(Assembly, path, Configuration);
		}
	}
}
