using FluentTranslate.Common.Domain;

namespace FluentTranslate.Parser
{
	public class FluentContext : Antlr4.Runtime.ParserRuleContext
	{
		public IFtlElement Element { get; set; }
	}
}