using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using File = System.IO.File;

namespace FluentTranslate.Infrastructure
{
	public class FluentInvariantLocalFileProvider : FluentInvariantFileProvider
	{
		public FluentInvariantLocalFileProvider(string path, IFluentConfiguration configuration)
			: base(path, configuration)
		{
		}

		protected override async Task<DateTime?> GetLastModifiedAsync(CultureInfo culture = null)
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

		protected override async Task<Timestamped<string>> FindFileAsync(CultureInfo culture = null)
		{
			try
			{
				using var stream = File.OpenRead(RequestPath);
				using var reader = new StreamReader(stream);
				var content = await reader.ReadToEndAsync();

				var lastWrite = File.GetLastWriteTime(RequestPath);
				return new Timestamped<string>(lastWrite, content);
			}
			catch (Exception)
			{

			}

			return null;
		}
	}
}
