using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluentTranslate.Infrastructure
{
	public class FluentHttpFileProvider : FluentFileProvider
	{
		private readonly HttpClient _client;

		protected HttpClient Client => _client
			?? Configuration.Services.GetService<HttpClient>();

		public FluentHttpFileProvider(string path, HttpClient client, IFluentConfiguration configuration)
			: base(path, configuration)
		{
			_client = client;
		}

		protected override TimeSpan GetPollingInterval()
		{
			return Configuration.Options.Get<FluentProviderOptions>()?.HttpPollingInterval
				?? TimeSpan.FromMinutes(1);
		}

		protected override bool CheckLastModified(Context context, CultureInfo culture = null) => false;

		protected override Task<DateTime?> GetLastModifiedAsync(Context context, CultureInfo culture = null)
		{
			throw new NotSupportedException();
		}

		protected override async Task<string> FindFileAsync(Context context, CultureInfo culture = null)
		{
			try
			{
				var response = await Client.GetAsync(RequestPath);
				if (!response.IsSuccessStatusCode)
					return null;

				var content = await response.Content.ReadAsStringAsync();
				return content;
			}
			catch (Exception)
			{

			}

			return null;
		}
	}
}
