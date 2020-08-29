using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Tree;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Parser
{
	public class FluentDeserializationVisitor : FluentParserBaseVisitor<IFluentElement>
	{
		public override IFluentElement Visit(IParseTree tree)
		{
			return base.Visit(tree);
		}

		public override IFluentElement VisitResource(FluentParser.ResourceContext context)
		{
			var resource = new FluentResource();
			context.Element = resource;
			base.VisitResource(context);
			return resource;
		}

		public override IFluentElement VisitEntry(FluentParser.EntryContext context)
        {
            return base.VisitEntry(context);
        }

		public override IFluentElement VisitMessage(FluentParser.MessageContext context)
		{
			var message = new FluentMessage();
			context.Element = message;
			base.VisitMessage(context);
			return message;
		}

		public override IFluentElement VisitText(FluentParser.TextContext context)
		{
			var containerContext = context.AscendParent().FirstOrDefault(x => x.Element is IFluentContainer);
			if (containerContext != null)
			{
				var container = (IFluentContainer)containerContext.Element;
				var inlineText = context.TEXT().GetText();
				var text = new FluentText()
				{
					Value = inlineText
				};
				container.Content.Add(text);
			}

			return base.VisitText(context);
		}

		public override IFluentElement VisitComment(FluentParser.CommentContext context)
		{
			return base.VisitComment(context);
		}
	}
}
