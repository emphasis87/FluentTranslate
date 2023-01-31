using Antlr4.Runtime;

using FluentTranslate.Domain;
using FluentTranslate.Parser;

namespace FluentTranslate.Serialization.Fluent
{
    public interface IFluentDeserializer
    {
        FluentDocument Deserialize(string content);
        IFluentElement Deserialize(string content, Func<FluentLexer, FluentParser, FluentParserContext> parse);
    }

    public class FluentDeserializer : IFluentDeserializer
    {
        public static FluentDeserializer Default { get; } = new FluentDeserializer();

        public FluentDocument Deserialize(string content)
        {
            return (FluentDocument)Deserialize(content, (_, x) => x.document());
        }

        public IFluentElement Deserialize(string content, Func<FluentLexer, FluentParser, FluentParserContext> parse)
        {
            var stream = new AntlrInputStream(new StringReader(content));
            var lexer = new FluentLexer(stream);
            var parser = new FluentParser(new CommonTokenStream(lexer));
            var context = parse(lexer, parser);
            var result = Deserialize(context);
            return result;
        }

        public IFluentElement Deserialize(FluentParserContext parserContext)
        {
            var visitor = new FluentParserVisitor();
            var result = visitor.Visit(parserContext);
            return result;
        }
    }
}
