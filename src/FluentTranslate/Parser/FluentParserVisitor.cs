using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using FluentTranslate.Domain;
using FluentTranslate.Domain.Common;
using System.Xml.Linq;

namespace FluentTranslate.Parser
{
	public class FluentParserVisitor : FluentParserBaseVisitor<IFluentElement>
	{
		private Queue<string> _texts = new();
        private StringBuilder _text = new();
        private Queue<string> _comments = new();
		private StringBuilder _ws = new();
        private int _commentLevel = 0;
        private Stack<IFluentElement> _items = new();
		private Stack<IFluentElement> _children = new();

        protected void AggregateItems(IFluentElement parent)
		{
            _children.Clear();

			IFluentElement current;
			while ((current = _items.Pop()) != parent)
			{
				_children.Push(current);
			}

			while(_children.Count > 0)
			{
				current = _children.Pop();
				if (_comments.Count > 0 )
				{
					switch(current)
					{
						case FluentComment comment when _commentLevel != comment.Level :
						case FluentMessage when _commentLevel != 1:
                            AddChild(new FluentComment(_commentLevel, PublishComment()));
							break;
						case FluentMessage message when _commentLevel == 1:
							message.Comment = PublishComment();
							break;
                    }
				}

				if (_texts.Count > 0)
				{
					if (current is not FluentText)
                        AddChild(new FluentText(PublishText()));
                }

				switch (current)
				{
					case FluentComment comment:
                        _comments.Enqueue(comment.Value);
                        _commentLevel = comment.Level;
						continue;
					case FluentText text:
                        _texts.Enqueue(text.Value);
						continue;
					case FluentEmptyLines:
						continue;
                }

                AddChild(current);
            }

			if (_texts.Count > 0)
				AddChild(new FluentText(PublishText()));
			if (_comments.Count > 0)
                AddChild(new FluentComment(_commentLevel, PublishComment()));

            _items.Push(parent);

            void AddChild(IFluentElement child)
			{
				switch (parent)
				{
					case IFluentAttributable attributable when child is FluentAttribute attribute:
						attributable.Attributes.Add(attribute);
						break;
					case IFluentContainer container when child is IFluentContent content:
						container.Add(content);
						break;
					case FluentPlaceable placeable:
                        switch (child)
                        {
                            case FluentPlaceable inner:
                                placeable.Content = inner.Content;
                                break;
                            case IFluentExpression expression:
                                placeable.Content = expression;
                                break;
                        }
						break;
					case FluentTermReference termReference when child is FluentCallArgument argument:
						termReference.Arguments.Add(argument);
						break;
					case FluentCallArgument argument when child is IFluentExpression expression:
						argument.Content = expression;
						break;
					case FluentDocument document when child is IFluentDocumentItem entry:
                        document.Add(entry);
						break;
                }
            }
		}

        private string PublishComment()
        {
            var comment = string.Join("\r\n", _comments);
            _comments.Clear();
            return comment;
        }

        private string PublishText()
        {
            // Find a common indentation
            _ws.Clear();
            int indent = int.MaxValue;
            foreach (var value in _texts.Skip(1))
            {
                int lineIndent = 0;
                int i = 0;
                while (lineIndent < indent && i < value.Length)
                {
                    char c = value[i++];
                    if (c == '\r' || c == '\n')
                    {
                        lineIndent = 0;
                        continue;
                    }

                    if (!char.IsWhiteSpace(c))
                        break;
                    if (_ws.Length == lineIndent)
                        _ws.Append(c);
                    else if (_ws[lineIndent] != c)
                        break;

                    lineIndent++;
                }
                indent = Math.Min(indent, lineIndent);
            }

            var start = _texts.Dequeue();
            _text.Append(start.TrimStart());

            foreach (var value in _texts)
            {
                int lineIndent = 0;
                for (var p = 0; p < value.Length; p++)
                {
                    char c = value[p];
                    if (c == '\r' || c == '\n')
                    {
                        _text.Append(c);
                        lineIndent = 0;
                        continue;
                    }

                    if (lineIndent < indent && _ws[lineIndent] == c)
                    {
                        lineIndent++;
                        continue;
                    }

                    _text.Append(value, p, value.Length - p);
                    break;
                }
            }

            var text = _text.ToString();
            _text.Clear();
            _texts.Clear();
            return text;
        }

        public override IFluentElement VisitChildren(IRuleNode node)
        {
            var val = DefaultResult;
            int childCount = node.ChildCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!ShouldVisitNextChild(node, val))
                {
                    break;
                }

				var child = node.GetChild(i);
                var nextResult = child.Accept(this);
                val = AggregateResult(val, nextResult);
            }

            return val;
        }

        protected virtual IFluentElement Publish<TResult>(TResult result)
            where TResult : IFluentElement
        {
            _items.Push(result);
            return result;
        }

        protected virtual IFluentElement ResolveChildren<T, TResult>(T context, TResult result)
			where T : FluentTranslate.Parser.FluentParserContext
			where TResult : IFluentElement
		{
            _items.Push(result);
            VisitChildren(context);
            AggregateItems(result);
            return result;
        }

        public override IFluentElement VisitDocument(FluentParser.DocumentContext context)
        {
            _items.Clear();
			return ResolveChildren(context, new FluentDocument());
        }

        public override IFluentElement VisitComment(FluentParser.CommentContext context)
		{
			var level = context.COMMENT_OPEN().GetText().TrimEnd().Length;
			var value = context.COMMENT().GetText();

			var result = new FluentComment
			{
				Level = level,
				Value = value,
			};

			return Publish(result);
		}

		public override IFluentElement VisitTerm(FluentParser.TermContext context)
		{
            var id = context.record().IDENTIFIER().GetText();
            return ResolveChildren(context, new FluentTerm(id));
        }

		public override IFluentElement VisitMessage(FluentParser.MessageContext context)
		{
            var id = context.record().IDENTIFIER().GetText();
            return ResolveChildren(context, new FluentMessage(id));
        }

        public override IFluentElement VisitAttribute(FluentParser.AttributeContext context)
        {
            var id = context.record().IDENTIFIER().GetText();
            return ResolveChildren(context, new FluentAttribute(id));
        }

        public override IFluentElement VisitEmptyLine(FluentParser.EmptyLineContext context)
        {
            if (_items.Count > 0 && _items.Peek() is FluentEmptyLines emptyLines)
                return emptyLines;

			return Publish(new FluentEmptyLines());
        }

        public override IFluentElement VisitText(FluentParser.TextContext context)
		{
			var value = context.GetText();
			return Publish(new FluentText(value));
		}

		public override IFluentElement VisitPlaceable(FluentParser.PlaceableContext context)
		{
			return ResolveChildren(context, new FluentPlaceable());

			//// Remove indentation text from inner placeable, e.g
			//// ... { indentation { $variable } } ...
			//result.RemoveAll(x => x is FluentText);

			//var child = result[0];
			//switch (child)
			//{
			//	case FluentPlaceable _:
			//	{
			//		// Inner placeable, e.g.
			//		// ... { { $variable } } ...
			//		break;
			//	}
			//	case IFluentExpression content:
			//	{
			//		var placeable = new FluentPlaceable {Content = content};
			//		result[0] = placeable;
			//		break;
			//	}
			//	default:
			//		throw UnsupportedChildTypeException(child);
			//}

			//// Add indentation prefix as a part of the expression, e.g.
			//// ... { $variable } indentation { "literal" } ...
			//var prefix = context.Prefix.GetText();
			//if (!string.IsNullOrEmpty(prefix))
			//{
			//	var text = new FluentText {Value = prefix};
			//	result.Insert(0, text);
			//}

			//return result;
		}

		public override IFluentElement VisitSelectExpression(FluentParser.SelectExpressionContext context)
		{
			return base.VisitSelectExpression(context);

			//Collector.AddType(FluentType.Selection);
			//base.VisitSelectExpression(context);
			//var result = Collector.AddSelection();

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
			//return result;
		}

		public override IFluentElement VisitDefaultVariant(FluentParser.DefaultVariantContext context)
		{
			return base.VisitDefaultVariant(context);
			/*
			var result = base.VisitDefaultVariant(context);
			var child = result.SingleOrDefault();
			switch (child)
			{
				case FluentVariant variant:
					variant.IsDefault = true;
					break;
			}

			return result;
			*/
		}

		public override IFluentElement VisitVariant(FluentParser.VariantContext context)
		{
			return base.VisitVariant(context);

			//Collector.AddType(FluentType.Variant);
			//base.VisitVariant(context);
			//var result = Collector.AddVariant();
			//return result;
		}

		public override IFluentElement VisitIdentifier(FluentParser.IdentifierContext context)
		{
			return base.VisitIdentifier(context);

			//Collector.AddType(FluentType.Identifier);
			//var id = context.GetText();
			//var result = Collector.AddReference(id);
			//return result;
		}

		public override IFluentElement VisitStringLiteral(FluentParser.StringLiteralContext context)
		{
            var value = context.GetText();
			// Remove quotes
			var literal = value.Substring(1, value.Length - 2);

			return Publish(new FluentStringLiteral(literal));
        }

		public override IFluentElement VisitNumberLiteral(FluentParser.NumberLiteralContext context)
		{
			var literal = context.GetText();
			return Publish(new FluentNumberLiteral(literal));
		}

		public override IFluentElement VisitVariableReference(FluentParser.VariableReferenceContext context)
		{
			var id = context.IDENTIFIER_REF().GetText();
			return Publish(new FluentVariableReference(id));
		}

        public override IFluentElement VisitMessageReference(FluentParser.MessageReferenceContext context)
        {
			return ResolveChildren(context, new FluentMessageReference());
        }

        public override IFluentElement VisitTermReference(FluentParser.TermReferenceContext context)
		{
			return ResolveChildren(context, new FluentTermReference());
		}

		public override IFluentElement VisitRecordReference(FluentParser.RecordReferenceContext context)
		{
			var id = context.IDENTIFIER_REF().GetText();
			var attributeId = context.attributeAccessor()?.IDENTIFIER_REF().GetText();
			
			base.VisitRecordReference(context);

			if (_items.Peek() is FluentRecordReference reference)
			{
				reference.Id = id;
				reference.AttributeId = attributeId;

                return reference;
			}

			return default!;
        }

        public override IFluentElement VisitFunctionCall(FluentParser.FunctionCallContext context)
		{
            var id = context.IDENTIFIER_REF().GetText();
			return ResolveChildren(context, new FluentFunctionCall(id));
		}

        public override IFluentElement VisitArgument(FluentParser.ArgumentContext context)
        {
			return ResolveChildren(context, new FluentCallArgument());
        }

        public override IFluentElement VisitInlineArgument(FluentParser.InlineArgumentContext context)
        {
            return base.VisitInlineArgument(context);
        }

        public override IFluentElement VisitNamedArgument(FluentParser.NamedArgumentContext context)
		{
			if (_items.Peek() is not FluentCallArgument argument)
				throw new Exception("There should be a FluentCallArgument on the stack.");

            var id = context.IDENTIFIER_REF().GetText();
			argument.Identifier = id;

            base.VisitNamedArgument(context);

			return argument;
		}
	}
}
