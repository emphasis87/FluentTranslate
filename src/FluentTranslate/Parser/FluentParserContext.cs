﻿using Antlr4.Runtime;

namespace FluentTranslate.Parser
{
    public class FluentParserContext : Antlr4.Runtime.ParserRuleContext
	{
		public FluentParserContext(ParserRuleContext parent, int invokingStateNumber) : base(parent, invokingStateNumber)
		{
		}
	}
}