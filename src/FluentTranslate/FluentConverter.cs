using FluentTranslate.Domain;
using FluentTranslate.Parser;
using FluentTranslate.Serialization.Fluent;

namespace FluentTranslate
{
	public class FluentConverter
	{
		public static FluentDocument Deserialize(string content)
		{
			return FluentDeserializer.Default.Deserialize(content);
		}

		public static IFluentElement Deserialize(string content, Func<FluentLexer, FluentParser, FluentParserContext> parse)
		{
			return FluentDeserializer.Default.Deserialize(content, parse);
		}

		public static string Serialize(FluentDocument resource)
		{
			return FluentSerializer.Default.Serialize(resource);
		}
	}
}
