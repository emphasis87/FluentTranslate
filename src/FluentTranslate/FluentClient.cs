using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace FluentTranslate
{
	public interface IFluentClient
	{
		string Translate(string message, IDictionary<string, object> parameters = null, CultureInfo culture = null);
	}

	public class FluentClient : IFluentClient
	{
		private readonly IFluentConfiguration _configuration;

		public Func<CultureInfo> DefaultCulture { get; set; }

		public FluentClient(IFluentConfiguration configuration = null)
		{
			_configuration = configuration;
		}

		public string Translate(string message, IDictionary<string, object> parameters = null, CultureInfo culture = null)
		{
			culture ??= DefaultCulture?.Invoke() ?? Thread.CurrentThread.CurrentCulture;
			return message;
		}
	}
}