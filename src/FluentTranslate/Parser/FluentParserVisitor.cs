using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using FluentTranslate.Domain;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Parser
{
	public class FluentParserVisitor : FluentParserBaseVisitor<IFluentElement>
	{
		private Stack<string> _texts = new();
        private StringBuilder _text = new();
        private Stack<string> _comments = new();
		private StringBuilder _comment = new();
        private int _commentLevel = 0;
        private Stack<IFluentElement> _items = new();
		private Stack<IFluentElement> _children = new();

        public override IFluentElement VisitDocument(FluentParser.DocumentContext context)
		{
			var document = new FluentDocument();

			_items.Clear();
            _items.Push(document);
			
			base.VisitDocument(context);

			AggregateItems(document);

			return document;
		}

		private void PublishComment()
		{
			if (_comments.Count > 0)
			{
                foreach (var value in _comments)
                    _comment.Append(value);

                var comment = new FluentComment(_commentLevel, _comment.ToString());

				_children.Push(comment);

				_comment.Clear();
				_commentLevel = 0;
			}
        }

        private void PublishText()
        {
			if (_texts.Count > 0)
			{
				// Find common indentation

				foreach (var value in _texts)
					_text.Append(value);

				var text = new FluentText(_text.ToString());
				
				_children.Push(text);

				_text.Clear();
				_texts.Clear();
			}
        }

        protected void AggregateItems(IFluentElement parent)
		{
            _children.Clear();
            IFluentElement current;
			while((current = _items.Pop()) != parent)
			{
				if (_comments.Count > 0 )
				{
					if (current is FluentComment c && _commentLevel != c.Level)
						PublishComment();
					if (current is FluentText && _commentLevel != 0)
						PublishComment();
				}

				if (_texts.Count > 0)
				{
					if (current is not FluentText)
						PublishText();
                }

				if (current is FluentComment comment)
				{
					_comments.Push(comment.Value);
					_commentLevel = comment.Level;
				}
				else if (current is FluentText text)
				{
					_texts.Push(text.Value);
				}
				else if (current is not FluentEmptyLines)
				{
					_children.Push(current);
				}
			}

			PublishText();
			PublishComment();

			if (parent is IFluentContainer container)
			{
				foreach(var child in _children.OfType<IFluentContent>())
					container.Add(child);
			} 
			else if (parent is FluentDocument document)
			{
                foreach (var child in _children.OfType<IFluentResourceEntry>())
                    document.Add(child);
            }

			_items.Push(parent);
		}

		public override IFluentElement VisitEmptyLine(FluentParser.EmptyLineContext context)
		{
			if (_items.Count > 0 && _items.Peek() is FluentEmptyLines emptyLines)
				return emptyLines;

			emptyLines = new FluentEmptyLines();

			_items.Push(emptyLines);

			return emptyLines;
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

			_items.Push(result);

			return result;
		}

		public override IFluentElement VisitTerm(FluentParser.TermContext context)
		{
            var term = new FluentTerm();

            _items.Push(term);

            base.VisitTerm(context);

            AggregateItems(term);

            return term;
        }

		public override IFluentElement VisitMessage(FluentParser.MessageContext context)
		{
			var message = new FluentMessage();

			_items.Push(message);

			base.VisitMessage(context);

            AggregateItems(message);

            return message;
        }

        public override IFluentElement VisitRecordHeader(FluentParser.RecordHeaderContext context)
        {
            var id = context.IDENTIFIER().GetText();

            if (_items.Peek() is not FluentRecord record)
                throw new Exception("There should be a FluentRecord type on the result stack.");

            record.Identifier = id;
            return record;
        }

        public override IFluentElement VisitAttribute(FluentParser.AttributeContext context)
		{
			var attribute = new FluentAttribute();

			_items.Push(attribute);

			base.VisitAttribute(context);

			return attribute;
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

		public override IFluentElement VisitText(FluentParser.TextContext context)
		{
			var value = context.GetText();
            var text = new FluentText() { Value = value };

			_items.Push(text);

			return text;
		}

		public override IFluentElement VisitPlaceable(FluentParser.PlaceableContext context)
		{
			return base.VisitPlaceable(context);
			/*
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
			*/
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
			//Collector.AddType(FluentType.TermReference);
			return base.VisitTermReference(context);
		}

		public override IFluentElement VisitRecordReference(FluentParser.RecordReferenceContext context)
		{
			var id = context.IDENTIFIER_REF().GetText();
			var attributeId = context.attributeAccessor()?.IDENTIFIER_REF().GetText();
			return base.VisitRecordReference(context);
			//var result = Collector.AddReference(id, attributeId);
			//return result;
		}

        public override IFluentElement VisitFunctionCall(FluentParser.FunctionCallContext context)
		{
			//Collector.AddType(FluentType.FunctionCall);
			base.VisitFunctionCall(context);
            var id = context.IDENTIFIER_REF().GetText();
			//var result = Collector.AddFunctionCall(id);
			//return result;
			return base.VisitFunctionCall(context);
		}

        public override IFluentElement VisitArgument([NotNull] FluentParser.ArgumentContext context)
        {
			var argument = new FluentCallArgument();
			_items.Push(argument);
            base.VisitArgument(context);
			return argument;
        }

        public override IFluentElement VisitInlineArgument([NotNull] FluentParser.InlineArgumentContext context)
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
