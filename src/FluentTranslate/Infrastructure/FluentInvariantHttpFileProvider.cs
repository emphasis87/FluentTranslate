using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentTranslate.Infrastructure
{
	public class FluentInvariantHttpFileProvider : FluentInvariantFileProvider
	{
		private readonly HttpClient _client;

		protected HttpClient Client => _client
			?? Configuration.Services.GetService<HttpClient>();

		public FluentInvariantHttpFileProvider(string path, HttpClient client, IFluentConfiguration configuration)
			: base(path, configuration)
		{
			_client = client;
		}

		protected override bool CheckLastModified(CultureInfo culture = null) => false;

		protected override Task<DateTime?> GetLastModifiedAsync(CultureInfo culture = null)
		{
			throw new NotSupportedException();
		}

		protected override async Task<Timestamped<string>> FindFileAsync(CultureInfo culture = null)
		{
			try
			{
				var response = await Client.GetAsync(RequestPath);
				if (!response.IsSuccessStatusCode)
					return null;

				var lastWrite = response.Content.Headers.LastModified?.DateTime ?? DateTime.Now;
				var content = await response.Content.ReadAsStringAsync();
				return new Timestamped<string>(lastWrite, content);
			}
			catch (Exception)
			{

			}

			return null;
		}
	}
}
