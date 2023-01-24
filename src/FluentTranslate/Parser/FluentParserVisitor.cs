using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using FluentTranslate.Common;
using FluentTranslate.Domain;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Parser
{
	public class FluentParserVisitor : FluentParserBaseVisitor<IFluentElement>
	{
		

		public override IFluentElement Visit(IParseTree tree)
		{
			return base.Visit(tree);
		}

		public FluentElementCollector Collector { get; private set; } = new FluentElementCollector();

		protected override IFluentElement DefaultResult => new FluentResource();

        public override IFluentElement VisitChildren(IRuleNode node)
        {
            IFluentElement val = DefaultResult;
            int childCount = node.ChildCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!ShouldVisitNextChild(node, val))
                {
                    break;
                }

                IFluentElement nextResult = node.GetChild(i).Accept(this);
                val = AggregateResult(val, nextResult);
            }

            return val;
        }

        public override IFluentElement VisitResource(FluentParser.ResourceContext context)
		{ 
			Collector.AddType(FluentType.Resource);
			base.VisitResource(context);
			var result = Collector.AddResource();
			return result;
			//if (result.Count == 0)
			//{
			//	result.Add(resource);
			//	return result;
			//}

			//AggregateSequential(result);

			//foreach (var child in result)
			//{
			//	switch (child)
			//	{
			//		case FluentEmptyLines _:
			//			// Ignore empty lines
			//			break;
			//		case IFluentResourceEntry entry:
			//			resource.Entries.Add(entry);
			//			break;
			//		default:
			//			throw UnsupportedChildTypeException(child);
			//	}
			//}

			//result.Clear();
			//result.Add(resource);
			//return result;
		}

        //private static void AggregateSequential<T>(IList<T> items)
        //{
        //    for (var i = 0; i < items.Count - 1; i++)
        //    {
        //        var right = items[i + 1];
        //        switch (items[i])
        //        {
        //            case IFluentAggregable aggregable when aggregable.CanAggregate(right):
        //            {
        //                items[i] = (T) aggregable.Aggregate(right);
        //                items.RemoveAt(i + 1);
        //                i--;
        //                break;
        //            }
        //        }
        //    }
        //}

        public override IFluentElement VisitEntry(FluentParser.EntryContext context)
		{
			return base.VisitEntry(context);
		}

		public override IFluentElement VisitEmptyLine(FluentParser.EmptyLineContext context)
		{
			Collector.AddEmptyLines();
			var empty = new FluentEmptyLines();
			return empty;
		}

		public override IFluentElement VisitComment(FluentParser.CommentContext context)
		{
			var level = context.COMMENT_OPEN().GetText().TrimEnd().Length;
			var value = context.COMMENT().GetText();

			Collector.AddComemnt(level, value);

			// This may be an incomplete comment
			var result = new FluentComment
			{
				Level = level,
				Value = value,
			};
			return result;
		}

		public override IFluentElement VisitTerm(FluentParser.TermContext context)
		{
			Collector.AddType(FluentType.Term);
			base.VisitTerm(context);
			var result = Collector.AddRecord();
			return result;

			//var record = (FluentRecord) result[0];
			//result.RemoveAt(0);
			
			//AggregateContainer(record, result);
			//AggregateRecord(record, result);

			//if (result.Count != 0)
			//	throw UnsupportedChildTypeException(result[0]);

			//result.Add(record);
			//return result;
		}

		public override IFluentElement VisitMessage(FluentParser.MessageContext context)
		{
            Collector.AddType(FluentType.Message);
            base.VisitMessage(context);
            var result = Collector.AddRecord();
            return result;

            //var result = base.VisitMessage(context);
            //var record = (FluentRecord) result[0];
            //result.RemoveAt(0);

            //AggregateContainer(record, result);
            //AggregateRecord(record, result);

            //if (result.Count != 0)
            //	throw UnsupportedChildTypeException(result[0]);

            //result.Add(record);
            //return result;
        }

		public override IFluentElement VisitAttribute(FluentParser.AttributeContext context)
		{
            Collector.AddType(FluentType.Attribute);
            base.VisitAttribute(context);
            var result = Collector.AddRecord();
            return result;

   //         var result = base.VisitAttribute(context);
			//var attribute = (FluentAttribute) result[0];
			//result.RemoveAt(0);

			//AggregateContainer(attribute, result);

			//if (result.Count != 0)
			//	throw UnsupportedChildTypeException(result[0]);

			//result.Add(attribute);
			//return result;
		}

		private static void AggregateRecord(FluentRecord record, IList<IFluentElement> result)
		{
			for (var i = 0; i < result.Count; i++)
			{
				switch (result[i])
				{
					case FluentAttribute attribute:
						record.Attributes.Add(attribute);
						result.RemoveAt(i);
						i--;
						break;
				}
			}
		}

		public override IFluentElement VisitRecord(FluentParser.RecordContext context)
		{
			base.VisitRecord(context);
			var id = context.IDENTIFIER().GetText();
			var result = Collector.AddRecord(id);
			return result;
		}

		public override IFluentElement VisitText(FluentParser.TextContext context)
		{
			var value = context.GetText();
			Collector.AddText(value);

			// This may be an incomplete text
            var result = new FluentText(value);

			return result;
		}

		public override IFluentElement VisitPlaceable(FluentParser.PlaceableContext context)
		{
			Collector.AddType(FluentType.Placeable);

			var result = base.VisitPlaceable(context);
			if (result.Count == 0)
				return result;

			// Remove indentation text from inner placeable, e.g
			// ... { indentation { $variable } } ...
			result.RemoveAll(x => x is FluentText);

			var child = result[0];
			switch (child)
			{
				case FluentPlaceable _:
				{
					// Inner placeable, e.g.
					// ... { { $variable } } ...
					break;
				}
				case IFluentExpression content:
				{
					var placeable = new FluentPlaceable {Content = content};
					result[0] = placeable;
					break;
				}
				default:
					throw UnsupportedChildTypeException(child);
			}

			// Add indentation prefix as a part of the expression, e.g.
			// ... { $variable } indentation { "literal" } ...
			var prefix = context.Prefix.GetText();
			if (!string.IsNullOrEmpty(prefix))
			{
				var text = new FluentText {Value = prefix};
				result.Insert(0, text);
			}

			return result;
		}

		public override IFluentElement VisitSelectExpression(FluentParser.SelectExpressionContext context)
		{
			Collector.AddType(FluentType.Selection);
			base.VisitSelectExpression(context);
			var result = Collector.AddSelection();
			//var selection = new FluentSelection();
			//foreach (var child in result)
			//{
			//	switch (child)
			//	{
			//		case IFluentExpression content:
			//			selection.Match = content;
			//			break;
			//		case FluentVariant variant:
			//			selection.Variants.Add(variant);
			//			break;
			//		default:
			//			throw UnsupportedChildTypeException(child);
			//	}
			//}

			//result.Clear();
			//result.Add(selection);
			return result;
		}

		public override IFluentElement VisitDefaultVariant(FluentParser.DefaultVariantContext context)
		{
			var result = base.VisitDefaultVariant(context);
			var child = result.SingleOrDefault();
			switch (child)
			{
				case FluentVariant variant:
					variant.IsDefault = true;
					break;
			}

			return result;
		}

		public override IFluentElement VisitVariant(FluentParser.VariantContext context)
		{
			Collector.AddType(FluentType.Variant);
			base.VisitVariant(context);
			var result = Collector.AddVariant();
			return result;
		}

		public override IFluentElement VisitIdentifier(FluentParser.IdentifierContext context)
		{
			Collector.AddType(FluentType.Identifier);
			var id = context.GetText();
			var result = Collector.AddReference(id);
			return result;
		}

		public override IFluentElement VisitStringLiteral(FluentParser.StringLiteralContext context)
		{
            var value = context.GetText();
			// Remove quotes
			var literal = value.Substring(1, value.Length - 2);
            var result = new FluentStringLiteral(literal);
            return result;
        }

		public override IFluentElement VisitNumberLiteral(FluentParser.NumberLiteralContext context)
		{
			var literal = context.GetText();
			var result = new FluentNumberLiteral(literal);
			return result;
		}

		public override IFluentElement VisitVariableReference(FluentParser.VariableReferenceContext context)
		{
			var id = context.IDENTIFIER_REF().GetText();
			var result = new FluentVariableReference(id);
            return result;
		}

        public override IFluentElement VisitMessageReference([NotNull] FluentParser.MessageReferenceContext context)
        {
            return base.VisitMessageReference(context);
        }

        public override IFluentElement VisitTermReference(FluentParser.TermReferenceContext context)
		{
			Collector.AddType(FluentType.TermReference);
			return base.VisitTermReference(context);
		}

		public override IFluentElement VisitRecordReference(FluentParser.RecordReferenceContext context)
		{
			var id = context.IDENTIFIER_REF().GetText();
			var attributeId = context.attributeAccessor()?.IDENTIFIER_REF().GetText();
			var result = Collector.AddReference(id, attributeId);
			return result;
		}

        public override IFluentElement VisitFunctionCall(FluentParser.FunctionCallContext context)
		{
			Collector.AddType(FluentType.FunctionCall);
			base.VisitFunctionCall(context);
            var id = context.IDENTIFIER_REF().GetText();
			var result = Collector.AddFunctionCall(id);
			return result;
		}

        public override IFluentElement VisitArgument([NotNull] FluentParser.ArgumentContext context)
        {
			var argument = new FluentCallArgument();
            return base.VisitArgument(context);
        }

        public override IFluentElement VisitInlineArgument([NotNull] FluentParser.InlineArgumentContext context)
        {
            base.VisitInlineArgument(context);
			var result = Collector.AddCallArgument();
			return result;
        }

        public override IFluentElement VisitNamedArgument(FluentParser.NamedArgumentContext context)
		{
			base.VisitNamedArgument(context);
			var id = context.IDENTIFIER_REF().GetText();
			var result = Collector.AddCallArgument(id);
			return result;
		}

        private static Exception UnsupportedChildTypeException(IFluentElement child, [CallerMemberName] string? callerName = null)
		{
			return new ArgumentOutOfRangeException(nameof(child), $"{callerName}: Child of type {child.GetType()} is not supported.");
		}

		private static Exception UnsupportedContextTypeException(RuleContext context, [CallerMemberName] string? callerName = null)
		{
			return new ArgumentOutOfRangeException(nameof(context), $"{callerName}: Context of type {context.GetType()} is not supported.");
		}
	}
}
