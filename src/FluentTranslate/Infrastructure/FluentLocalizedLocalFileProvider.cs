namespace FluentTranslate.Infrastructure
{
	public class FluentLocalizedLocalFileProvider : FluentLocalizedFileProvider
	{
		public FluentLocalizedLocalFileProvider(string rootPath, IFluentConfiguration configuration)
			: base(rootPath, configuration)
		{

		}

		protected override IFluentProvider CreateProvider(string path)
		{
			return new FluentLocalFileProvider(path, Configuration);
		}
	}
}