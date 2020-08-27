﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Tree;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Parser
{
	public class FtlVisitor : FluentParserBaseVisitor<IFtlElement>
	{
		public override IFtlElement Visit(IParseTree tree)
		{
			return base.Visit(tree);
		}

		public override IFtlElement VisitResource(FluentParser.ResourceContext context)
		{
			var resource = new FtlResource();
			context.Element = resource;
			base.VisitResource(context);
			return resource;
		}

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

		public override IFtlElement VisitTextInline(FluentParser.TextInlineContext context)
		{
			var containerContext = context.AscendParent().FirstOrDefault(x => x.Element is IFtlContentEntry);
			if (containerContext != null)
			{
				var container = (IFtlContentEntry) containerContext.Element;
				var inlineText = context.TEXT_INLINE().GetText();
				var text = new FtlText()
				{
					Value = inlineText
				};
				container.Content.Add(text);
			}

			return base.VisitTextInline(context);
		}

		public override IFtlElement VisitComment(FluentParser.CommentContext context)
		{
			return base.VisitComment(context);
		}
	}
}
