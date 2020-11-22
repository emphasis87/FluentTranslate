using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using File = System.IO.File;

namespace FluentTranslate.Infrastructure
{
	public class FluentLocalFileProvider : FluentFileProvider
	{
		public FluentLocalFileProvider(string path, IFluentConfiguration configuration = null)
			: base(path, configuration)
		{
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
