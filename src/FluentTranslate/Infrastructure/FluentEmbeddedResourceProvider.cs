using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public class FluentEmbeddedResourceProvider : IFluentResourceProvider
	{
		private readonly Assembly _assembly;
		private readonly string _path;
		private readonly Lazy<FluentResource> _resource;

		public FluentEmbeddedResourceProvider(Assembly assembly, string path)
		{
			_assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
			_path = path ?? throw new ArgumentNullException(nameof(path));
			_resource = new Lazy<FluentResource>(FindResource);
		}

		public Task<FluentResource> GetResourceAsync(CultureInfo culture = null)
		{
			return Task.FromResult(_resource.Value);
		}

		private FluentResource FindResource()
		{
			using var stream = _assembly.GetManifestResourceStream(_path);
			if (stream is null)
				throw new ArgumentException($"Unable to find embedded resource '{_path}' in {_assembly}");

			using var reader = new StreamReader(stream);
			var source = reader.ReadToEnd();
			var result = FluentConverter.Deserialize(source);
			return result;
		}
	}
}
