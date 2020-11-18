using System.Globalization;

namespace FluentTranslate
{
	public interface IFluentEngine
	{
		string Evaluate(string message, CultureInfo culture);
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