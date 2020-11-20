using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public abstract class FluentInvariantFileProvider : FluentInvariantProvider
	{
		protected string RequestPath { get; }
		protected DateTime? LastModified { get; private set; }

		protected IFluentDeserializerContainer Deserializers =>
			Configuration.Services.GetService<IFluentDeserializerContainer>() ?? FluentDeserializerContainer.Default;

		protected FluentInvariantFileProvider(string path, IFluentConfiguration configuration) 
			: base(configuration)
		{
			RequestPath = path ?? throw new ArgumentNullException(nameof(path));
		}

		protected virtual bool CheckLastModified(CultureInfo culture = null) => true;

		protected override async Task<Timestamped<FluentResource>> FindResourceAsync(
			DateTime now,
			Timestamped<FluentResource> lastResult, 
			CultureInfo culture)
		{
			if (CheckLastModified(culture))
			{
				var lastModified = await GetLastModifiedAsync(culture);
				if (lastModified == LastModified || lastModified < LastModified)
					return lastResult;
			}

			var next = await FindFileAsync(culture);

			var nextContent = next?.Value;
			if (nextContent is null)
			{
				LastModified = now;
				return new Timestamped<FluentResource>(now, null);
			}

			var nextTimestamp = next.Timestamp;
			var nextResource = Deserialize(nextContent);
			LastModified = nextTimestamp;
			return new Timestamped<FluentResource>(nextTimestamp, nextResource);
		}

		protected abstract Task<DateTime?> GetLastModifiedAsync(CultureInfo culture = null);

		protected abstract Task<Timestamped<string>> FindFileAsync(CultureInfo culture = null);

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