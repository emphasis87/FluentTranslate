namespace FluentTranslate.Infrastructure
{
	public class FluentLocalizedLocalFileProvider : FluentLocalizedFileProvider
	{
		public FluentLocalizedLocalFileProvider(string rootPath, IFluentConfiguration configuration = null)
			: base(rootPath, configuration)
		{

		}

		protected override IFluentResourceProvider CreateProvider(string path)
		{
			return new FluentLocalFileProvider(path, Configuration);
		}
	}
}