using System;
using System.Collections.Generic;
using FluentTranslate.Domain;
using FluentTranslate.Parser;
using FluentTranslate.Serialization.Fluent;

namespace FluentTranslate
{
    public class FluentConverter
	{
		public static FluentResource Deserialize(string content)
		{
			return FluentDeserializer.Default.Deserialize(content);
		}

		public static List<IFluentElement> Deserialize(string content, Func<FluentLexer, FluentParser, FluentParserContext> parse)
		{
			return FluentDeserializer.Default.Deserialize(content, parse);
		}

		public static string Serialize(FluentResource resource)
		{
			return FluentSerializer.Default.Serialize(resource);
		}
	}
}
