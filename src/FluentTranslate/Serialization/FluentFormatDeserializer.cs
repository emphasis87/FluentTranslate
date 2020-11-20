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
	}

	public class FluentFormatDeserializer : IFluentDeserializer
	{
		public static FluentFormatDeserializer Default { get; } = new FluentFormatDeserializer();

		public FluentResource Deserialize(string content)
		{
			var stream = new AntlrInputStream(new StringReader(content));
			var lexer = new FluentLexer(stream);
			var parser = new FluentParser(new CommonTokenStream(lexer));

			var visitor = new FluentFormatDeserializationVisitor();
			var resource = visitor.Visit(parser.resource()).FirstOrDefault() as FluentResource;

			return resource;
		}
	}
}
