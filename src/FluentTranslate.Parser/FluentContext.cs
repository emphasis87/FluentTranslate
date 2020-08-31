using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace FluentTranslate.Parser
{
	public class FluentContext : Antlr4.Runtime.ParserRuleContext
	{
		public FluentContext(ParserRuleContext parent, int invokingStateNumber) : base(parent, invokingStateNumber)
		{
		}
	}
}