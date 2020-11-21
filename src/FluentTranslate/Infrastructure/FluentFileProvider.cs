using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public abstract class FluentFileProvider : FluentProvider
	{
		protected string RequestPath { get; }

		private DateTime? _lastModified;

		protected IFluentDeserializerContainer Deserializers =>
			Configuration.Services.GetService<IFluentDeserializerContainer>() ?? FluentDeserializerContainer.Default;

		protected FluentFileProvider(string path, IFluentConfiguration configuration) 
			: base(configuration)
		{
			RequestPath = path ?? throw new ArgumentNullException(nameof(path));
		}

		protected override async Task<FluentResource> FindResourceAsync(Context context, CultureInfo culture)
		{
			if (CheckLastModified(context, culture))
			{
				var lastModified = await GetLastModifiedAsync(context, culture);
				if (lastModified == _lastModified || lastModified < _lastModified)
					return null; // No change

				_lastModified = lastModified;
			}

			var content = await FindFileAsync(context, culture);
			var result = Deserialize(content);
			return result ?? new FluentResource();
		}

		protected virtual bool CheckLastModified(Context context, CultureInfo culture) => true;
		protected abstract Task<DateTime?> GetLastModifiedAsync(Context context, CultureInfo culture = null);

		protected abstract Task<string> FindFileAsync(Context context, CultureInfo culture = null);

		private FluentResource Deserialize(string content)
		{
			if (content is null)
				return null;

			var extension = Path.GetExtension(RequestPath);
			var deserializer = Deserializers.Get(extension);
			if (deserializer is null)
				throw new InvalidOperationException($"Extension '{extension}' is not supported for deserialization.");

			var resource = deserializer.Deserialize(content);
			return resource;
		}
	}
}