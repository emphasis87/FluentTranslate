using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public abstract class FluentFileResourceProvider : FluentPollingResourceProvider
	{
		protected string RequestPath { get; }

		private DateTime? _lastModified = DateTime.MinValue;

		protected FluentFileResourceProvider(string path, IFluentConfiguration configuration = null) 
			: base(configuration)
		{
			RequestPath = path ?? throw new ArgumentNullException(nameof(path));
		}

		protected override async Task<FluentDocument> FindResourceAsync(Context context, CultureInfo culture)
		{
			if (CheckLastModified(context, culture))
			{
				var lastModified = await GetLastModifiedAsync(context, culture);
				if (lastModified == _lastModified || lastModified < _lastModified)
					return null; // No change

				_lastModified = lastModified;
			}

			var content = await FindFileAsync(context, culture);
			var extension = Path.GetExtension(RequestPath);
			var result = Configuration.Deserialize(content, extension);
			return result ?? new FluentDocument();
		}

		protected virtual bool CheckLastModified(Context context, CultureInfo culture = null) => true;
		protected abstract Task<DateTime?> GetLastModifiedAsync(Context context, CultureInfo culture = null);

		protected abstract Task<string> FindFileAsync(Context context, CultureInfo culture = null);
	}
}