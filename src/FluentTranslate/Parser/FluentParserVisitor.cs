using Antlr4.Runtime.Tree;
using FluentTranslate.Domain;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Parser
{
    public class FluentParserVisitor : FluentParserBaseVisitor<IFluentElement?>
	{
		private Queue<string> _texts = new();
        private StringBuilder _text = new();
        private Queue<string> _comments = new();
		private StringBuilder _ws = new();
        private int _commentLevel = 0;
        private Stack<IFluentElement> _items = new();
		private Stack<IFluentElement> _children = new();

        protected void AggregateItems<T>(IFluentElement parent, Action<T> addChild)
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
						case FluentRecord when _commentLevel != 1:
                            AddChild(new FluentComment(_commentLevel, PublishComment()));
							break;
						case FluentRecord record when _commentLevel == 1:
							record.Comment = PublishComment();
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
                if (child is T item)
                    addChild(item);
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
            int lineIndent = 0;
            foreach (var value in _texts.Skip(1))
            {
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
            _text.Append(start);

            lineIndent = 0;
            foreach (var value in _texts)
            {
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
            if (node is null)
                return val!;

            int childCount = node.ChildCount;
            for (int i = 0; i < childCount; i++)
            {
                if (!ShouldVisitNextChild(node, val))
                    break;

				var child = node.GetChild(i);
                var nextResult = child.Accept(this);
                val = AggregateResult(val, nextResult);
            }

            return val!;
        }

        protected virtual IFluentElement Publish<TResult>(TResult result)
            where TResult : IFluentElement
        {
            _items.Push(result);
            return result;
        }

        protected IFluentElement Resolve<T, TChild>(T context, IFluentElement result, ICollection<TChild> children, bool accept = true)
			where T : FluentTranslate.Parser.FluentParserContext
		{
            return Resolve(context, result, (TChild x) => children.Add(x), accept);
        }

        protected IFluentElement Resolve<T>(T context, IFluentElement result, bool accept = true)
            where T : FluentTranslate.Parser.FluentParserContext
        {
            return Resolve<T, IFluentElement>(context, result, accept: accept);
        }

        protected virtual IFluentElement Resolve<T, TChild>(T context, IFluentElement result, Action<TChild>? addChild = null, bool accept = true)
            where T : FluentTranslate.Parser.FluentParserContext
        {
            if (_items.Count == 0 || _items.Peek() != result)
                _items.Push(result);

            if (context is null)
                return result;

            if (accept)
                context.Accept(this);
            else
                VisitChildren(context);

            if (addChild is not null)
                AggregateItems(result, addChild);
            
            return result;
        }

        public override IFluentElement VisitDocument(FluentParser.DocumentContext context)
        {
            _items.Clear();
            var document = new FluentDocument();
            return Resolve(context.Content, document, document.Content);
        }

        public override IFluentElement VisitComment(FluentParser.CommentContext context)
		{
			var level = context.COMMENT_OPEN().GetText().TrimEnd().Length;
			var value = context.COMMENT().GetText();
			return Publish(new FluentComment(level, value));
		}

		public override IFluentElement VisitTerm(FluentParser.TermContext context)
		{
            var id = context.record().IDENTIFIER().GetText();
            var term = new FluentTerm(id);
            Resolve(context.Content, term, term.Content);
            Resolve(context.Attributes, term, term.Attributes);
            return term;
        }

		public override IFluentElement VisitMessage(FluentParser.MessageContext context)
		{
            var id = context.record().IDENTIFIER().GetText();
            var message = new FluentMessage(id);
            Resolve(context.Content, message, message.Content);
            Resolve(context.Attributes, message, message.Attributes);
            return message;
        }

        public override IFluentElement VisitAttribute(FluentParser.AttributeContext context)
        {
            var id = context.record().IDENTIFIER().GetText();
            var attribute = new FluentAttribute(id);
            Resolve(context.Content, attribute, attribute.Content);
            return attribute;
        }

        public override IFluentElement VisitEmptyLine(FluentParser.EmptyLineContext context)
        {
            if (_items.Count > 0 && _items.Peek() is FluentEmptyLines emptyLines)
                return emptyLines;

			return Publish(new FluentEmptyLines());
        }

        public override IFluentElement? VisitText(FluentParser.TextContext context)
		{
			var value = context.GetText();
            return Publish(new FluentText(value));
		}

        public override IFluentElement VisitBlank(FluentParser.BlankContext context)
        {
            var value = context.GetText();
            if (_items.Count > 0 && _items.Peek() is IFluentContainer container && container.Content.Count == 0)
                return default!;
            return Publish(new FluentText(value));
        }

        public override IFluentElement VisitPlaceable(FluentParser.PlaceableContext context)
		{
            var placeable = new FluentPlaceable();
            Resolve(context.Content, placeable, (IFluentExpression content) =>
                placeable.Content = content switch
                {
                    FluentPlaceable inner => inner.Content,
                    _ => content,
                });
            return placeable;
		}

		public override IFluentElement VisitSelectExpression(FluentParser.SelectExpressionContext context)
		{
            var selection = new FluentSelection();
            Resolve(context.Match, selection, (IFluentExpression match) =>
                selection.Match = match);
            Resolve(context.Variants, selection, selection.Variants);
            return selection;
        }

		public override IFluentElement VisitDefaultVariant(FluentParser.DefaultVariantContext context)
		{
            var result = base.VisitDefaultVariant(context);
            if (result is FluentVariant variant)
            {
                variant.IsDefault = true;
                return variant;
            }
            return result!;
		}

		public override IFluentElement VisitVariant(FluentParser.VariantContext context)
		{
            var variant = new FluentVariant();
            Resolve(context.Key, variant, (FluentVariantKey key) =>
                variant.Key = key);
            Resolve(context.Content, variant, variant.Content);
            return variant;
        }

        public override IFluentElement VisitVariantKey(FluentParser.VariantKeyContext context)
        {
            var key = new FluentVariantKey();
            Resolve(context, key, accept: false, addChild: (IFluentVariantIdentifier identifier) =>
                key.Identifier = identifier);
            return key;
        }

        public override IFluentElement VisitIdentifier(FluentParser.IdentifierContext context)
		{
            var id = context.GetText();
			return Publish(new FluentIdentifier(id));
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

        private (string id, string? attributeId) ResolveRecordReference(FluentParser.RecordReferenceContext context)
        {
            var id = context.IDENTIFIER_REF().GetText();
            var attributeId = context.attributeAccessor()?.IDENTIFIER_REF().GetText();
            return (id, attributeId);
        }

        public override IFluentElement VisitMessageReference(FluentParser.MessageReferenceContext context)
        {
            var (id, attributeId) = ResolveRecordReference(context.recordReference());
            return Publish(new FluentMessageReference(id, attributeId));
        }

        public override IFluentElement VisitTermReference(FluentParser.TermReferenceContext context)
		{
            var (id, attributeId) = ResolveRecordReference(context.recordReference());
            var reference = new FluentTermReference(id, attributeId);
            return Resolve(context.argumentList(), reference, reference.Arguments);
        }

        public override IFluentElement VisitFunctionCall(FluentParser.FunctionCallContext context)
		{
            var id = context.IDENTIFIER_REF().GetText();
            var call = new FluentFunctionCall(id);
            Resolve(context.argumentList(), call, call.Arguments);
            return call;
		}

        public override IFluentElement VisitArgument(FluentParser.ArgumentContext context)
        {
            var argument = new FluentCallArgument();
            var id = context.argumentName()?.IDENTIFIER_REF().GetText();
            if (id is not null)
                argument.Identifier = id;
            Resolve(context.Argument, argument, (IFluentExpression content) =>
                argument.Content = content);
            return argument;
        }
	}
}
