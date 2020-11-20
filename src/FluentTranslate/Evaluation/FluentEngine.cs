using System.Collections.Generic;
using System.Globalization;

namespace FluentTranslate.Evaluation
{
	public interface IFluentEngine
	{
		string Evaluate(string message, IDictionary<string, object> parameters, CultureInfo culture);
	}

	public class FluentEngine
	{
		private readonly IFluentConfiguration _configuration;

		public FluentEngine(IFluentConfiguration configuration)
		{
			_configuration = configuration;
		}
	}
}