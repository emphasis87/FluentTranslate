using System;
using System.Collections.Generic;
using System.Text;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Parser
{
	public class FtlVisitor : FluentBaseVisitor<IFtlEntry[]>
	{


		public override IFtlEntry[] VisitEntry(FluentParser.EntryContext context)
		{
			return base.VisitEntry(context);
		}
	}
}
