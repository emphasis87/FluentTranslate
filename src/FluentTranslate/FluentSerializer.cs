using FluentTranslate.Domain;
using FluentTranslate.Parser;
using FluentTranslate.Serialization.Fluent;

namespace FluentTranslate
{
	public class FluentSerializer
	{
		public static FluentResource Deserialize(string content)
		{
			return FluentDeserializer.Default.Deserialize(content);
		}

		public static IFluentElement Deserialize(string content, Func<FluentLexer, FluentParser, FluentParserContext> parse)
		{
			return FluentDeserializer.Default.Deserialize(content, parse);
		}

		public static string Serialize(FluentResource resource)
		{
			return Serialization.Fluent.FluentSerializer.Default.Serialize(resource);
		}
	}
}
