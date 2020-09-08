using System.IO;
using System.Linq;
using Antlr4.Runtime;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Parser
{
	public class FluentDeserializer
	{
		public static FluentResource Deserialize(string content, FluentLexer lexer = null, FluentParser parser = null)
		{
			lexer ??= new FluentLexer(new AntlrInputStream(new StringReader(content)));
			parser ??= new FluentParser(new CommonTokenStream(lexer));

			var visitor = new FluentDeserializationVisitor();
			return visitor.Visit(parser.resource()).FirstOrDefault() as FluentResource;
		}
	}
}
