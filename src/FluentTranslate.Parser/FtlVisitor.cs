using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Tree;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Parser
{
	public class FtlVisitor : FluentBaseVisitor<IFtlElement>
	{	
        public override IFtlElement VisitEntry(FluentParser.EntryContext context)
        {
            return base.VisitEntry(context);
        }

		public override IFtlElement VisitMessage(FluentParser.MessageContext context)
		{
			var message = new FtlMessage();
			context.Element = message;
			base.VisitMessage(context);
			return message;
		}

		public override IFtlElement VisitAttribute(FluentParser.AttributeContext context)
		{
			var attribute = new FtlAttribute();
			context.Element = attribute;
			if (context.Parent is FluentContext parentContext)
			{
				switch (parentContext.Element)
				{
					case IFtlContentEntry attributeElement:
						attributeElement.Attributes.Add(attribute);
						break;
				}
			}

			base.VisitAttribute(context);
			return attribute;
		}
	}
}
