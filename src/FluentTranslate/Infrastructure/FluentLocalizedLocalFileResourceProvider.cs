namespace FluentTranslate.Infrastructure
{
	public class FluentLocalizedLocalFileResourceProvider : FluentLocalizedFileResourceProvider
	{
		public FluentLocalizedLocalFileResourceProvider(string rootPath, IFluentConfiguration configuration = null)
			: base(rootPath, configuration)
		{

		}

		protected override IFluentResourceProvider CreateProvider(string path)
		{
			return new FluentLocalFileResourceProvider(path, Configuration);
		}
	}
}