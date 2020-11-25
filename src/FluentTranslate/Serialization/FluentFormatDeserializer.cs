using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using FluentTranslate.Domain;
using FluentTranslate.Parser;

namespace FluentTranslate.Serialization
{
	public interface IFluentDeserializer
	{
		FluentResource Deserialize(string content);
		List<IFluentElement> Deserialize(string content, Func<FluentLexer, FluentParser, FluentParserContext> parse);
	}

	public class FluentFormatDeserializer : IFluentDeserializer
	{
		public static FluentFormatDeserializer Default { get; } = new FluentFormatDeserializer();

		public FluentResource Deserialize(string content)
		{
			return (FluentResource) Deserialize(content, (_, x) => x.resource()).FirstOrDefault();
		}

		public List<IFluentElement> Deserialize(string content, Func<FluentLexer, FluentParser, FluentParserContext> parse)
		{
			var stream = new AntlrInputStream(new StringReader(content));
			var lexer = new FluentLexer(stream);
			var parser = new FluentParser(new CommonTokenStream(lexer));
			var context = parse(lexer, parser);
			var result = Deserialize(context);
			return result;
		}

		public List<IFluentElement> Deserialize(FluentParserContext parserContext)
		{
			var visitor = new FluentFormatDeserializationVisitor();
			var result = visitor.Visit(parserContext);
			return result;
		}
	}
}
