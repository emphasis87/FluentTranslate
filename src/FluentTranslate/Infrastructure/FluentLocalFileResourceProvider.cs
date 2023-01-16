using File = System.IO.File;

namespace FluentTranslate.Infrastructure
{
	public class FluentLocalFileResourceProvider : FluentFileResourceProvider
	{
		public FluentLocalFileResourceProvider(string path, IFluentConfiguration configuration = null)
			: base(path, configuration)
		{
		}

		protected override TimeSpan GetPollingInterval()
		{
			return Configuration?.Options.Get<FluentResourceProviderOptions>()?.FilePollingInterval
				?? TimeSpan.FromSeconds(5);
		}

		protected override async Task<DateTime?> GetLastModifiedAsync(Context context, CultureInfo culture = null)
		{
			if (!File.Exists(RequestPath))
				return null;

			try
			{
				var lastWrite = File.GetLastWriteTime(RequestPath);
				return lastWrite;
			}
			catch (Exception)
			{

			}

			return null;
		}

		protected override async Task<string> FindFileAsync(Context context, CultureInfo culture = null)
		{
			try
			{
				using var stream = File.OpenRead(RequestPath);
				using var reader = new StreamReader(stream);
				var content = await reader.ReadToEndAsync();
				return content;
			}
			catch (Exception)
			{

			}

			return null;
		}
	}
}
