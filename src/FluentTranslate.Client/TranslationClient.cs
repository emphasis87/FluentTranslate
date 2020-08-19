using System;
using System.Collections.Generic;

namespace FluentTranslate.Client
{
	public interface ITranslationClient
	{
		string GetString(string key, IDictionary<string, object> variables);
	}

	public class TranslationClient : ITranslationClient
	{
		public string GetString(string key, IDictionary<string, object> variables)
		{
			throw new NotImplementedException();
		}
	}
}
