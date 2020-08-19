using System.Linq;

namespace FluentTranslate.Client
{
	public static class TranslateExtensions
	{
		public static string GetString(this ITranslationClient client, string key, params (string variable, object value)[] variables)
		{
			return client.GetString(key, variables.ToDictionary(x => x.variable, x => x.value));
		}
	}
}