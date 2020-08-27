﻿using System.Collections.Generic;
using Antlr4.Runtime;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Parser
{
	public class FluentContext : Antlr4.Runtime.ParserRuleContext
	{
		public IFtlElement Element { get; set; }

		public FluentContext(ParserRuleContext parent, int invokingStateNumber) : base(parent, invokingStateNumber)
		{
		}

		public IEnumerable<FluentContext> AscendParent()
		{
			var parent = Parent;
			while (parent is FluentContext fc)
			{
				yield return fc;
				parent = parent.Parent;
			}
		}
	}
}