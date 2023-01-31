using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public class FluentEmbeddedResourceProvider : IFluentResourceProvider
	{
		protected IFluentConfiguration Configuration { get; }

		protected Assembly Assembly { get; }
		protected string ResourcePath { get; }

		private readonly Lazy<FluentDocument> _resource;

		public FluentEmbeddedResourceProvider(Assembly assembly, string path, IFluentConfiguration configuration = null)
		{
			Configuration = configuration;

			Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
			ResourcePath = path ?? throw new ArgumentNullException(nameof(path));
			_resource = new Lazy<FluentDocument>(FindResource);
		}

		public Task<FluentDocument> GetResourceAsync(CultureInfo culture = null)
		{
			return Task.FromResult(_resource.Value);
		}

		private FluentDocument FindResource()
		{
			using var stream = Assembly.GetManifestResourceStream(ResourcePath);
			if (stream is null)
			{
				//throw new ArgumentException($"Unable to find embedded resource '{ResourcePath}' in {Assembly}");
				return null;
			}

			using var reader = new StreamReader(stream);
			var source = reader.ReadToEnd();
			var extension = Path.GetExtension(ResourcePath);
			var result = Configuration.Deserialize(source, extension);
			return result;
		}
	}
}
