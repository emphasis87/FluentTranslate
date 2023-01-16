﻿using FluentTranslate.Common;
using FluentTranslate.Domain;
using FluentTranslate.Domain.Common;
using System.ComponentModel;

namespace FluentTranslate.Parser
{
    public class FluentElementCollector : IFluentElement
    {
        public string Type => throw new NotImplementedException();

        public Stack<string> Types { get; set; } = new();
        public Stack<IFluentElement> Elements { get; set; } = new();
        public List<string> Texts { get; } = new();
        public List<int> Levels { get; set; } = new();

        public FluentElementCollector() 
        {

        }

        public void AddType(string type)
        {
            Types.Push(type);
        }

        public void AddEmptyLines()
        {
            Types.Push(FluentTypes.EmptyLines);
        }

        public FluentResource AddResource()
        {
            throw new NotImplementedException();
        }

        public IFluentElement AddRecord(string? id = null)
        {
            var type = Types.Pop();
            id ??= "";
            IFluentElement result = type switch
            {
                FluentTypes.Message => new FluentMessage(id),
                FluentTypes.Term => new FluentTerm(id),
                FluentTypes.Attribute => new FluentAttribute(id),
                FluentTypes.Identifier => new FluentIdentifier(id),
                _ => throw UnsupportedChildTypeException(type),
            };
            Elements.Push(result);
            return result;
        }

        public IFluentElement AddSelection()
        {
            throw new NotImplementedException();
        }

        public IFluentElement AddFunctionCall(string id)
        {
            var result = new FluentFunctionCall(id);
            while (Types.Peek() == FluentTypes.CallArgument)
            {
                var next = GetElement<FluentCallArgument>();
                result.Arguments.Add(next);
            }
            result.Arguments.Reverse();
            Elements.Push(result);
            return result;
        }

        public IFluentElement AddCallArgument(string? id = null)
        {
            var content = GetElement<IFluentExpression>();
            var argument = new FluentCallArgument(id)
            {
                Content = content
            };
            Elements.Push(argument);
            return argument;
        }

        public void AddComemnt(int level, string value)
        {
            Types.Push(FluentTypes.Comment);
            Levels.Add(level);
            Texts.Add(value);
        }

        public void AddText(string value)
        {
            if (Types.Peek() != FluentTypes.Text)
                Types.Push(FluentTypes.Text);

            Texts.Add(value);
        }

        public IFluentElement AddReference(string id, string? attributeId = null)
        {
            var type = Types.Pop();
            IFluentElement result = type switch
            {
                FluentTypes.MessageReference => new FluentMessageReference(id),
                FluentTypes.TermReference => new FluentTermReference(id, attributeId),
                FluentTypes.VariableReference => new FluentVariableReference(id),
                FluentTypes.Identifier => new FluentIdentifier(id),
                _ => throw UnsupportedChildTypeException(type),
            };
            Elements.Push(result);
            return result;
        }

        public IFluentElement AddLiteral(string value)
        {
            var type = Types.Pop();
            IFluentElement result = type switch
            {
                FluentTypes.StringLiteral => new FluentStringLiteral(value),
                FluentTypes.NumberLiteral => new FluentNumberLiteral(value),
                _ => throw UnsupportedChildTypeException(type),
            };
            Elements.Push(result);
            return result;
        }

        protected IFluentElement GetNext()
        {
            return Types.Peek() switch
            {
                FluentTypes.Text => 
            }
        }

        private static readonly Regex WhitespaceIndent = new(@"[\r\n\u0020]*?\r?\n(\u0020*[^\r\n\u0020])", RegexOptions.Compiled | RegexOptions.Singleline);
        private static Regex IndentationPattern(int indentation) => new($@"(\r?\n)\u0020{{1,{indentation}}}", RegexOptions.Compiled | RegexOptions.Singleline);

        protected FluentText CollectText()
        {
            var count = 0;
            while(Types.Peek() == FluentTypes.Text)
            {
                Types.Pop();
                count++;
            }
            var texts = Texts.GetRange(Texts.Count - count, count);

            // Find the smallest common indentation on each line
            var indentation = int.MaxValue;
            foreach (var text in texts)
            {
                var matches = WhitespaceIndent.Matches(text);
                if (matches.Count != 0)
                {
                    foreach (Match match in matches)
                        indentation = Math.Min(indentation, match.Groups[1].Length - 1);
                }
            }

            // Remove common indentation from each line
            if (indentation < int.MaxValue)
            {
                var indentationPattern = IndentationPattern(indentation);
                foreach (var text in indented)
                    text.Value = indentationPattern.Replace(text.Value, "$1");
            }

            // Remove whitespace indentation from the first text
            while (container.Content.FirstOrDefault() is FluentText firstText)
            {
                firstText.Value = firstText.Value.TrimStart('\r', '\n', ' ');
                if (!string.IsNullOrEmpty(firstText.Value))
                    break;

                container.Content.RemoveAt(0);
            }


            var content = string.Join("\r\n", texts);
            var result = new FluentText(content);
            Elements.Push(result);
            return result;

            
        }

        protected T GetElement<T>()
        {
            Types.Pop();
            var element = Elements.Pop();
            if (element is T result)
                return result;

            throw UnsupportedChildTypeException(element.Type);
        }

        private static Exception UnsupportedChildTypeException(string type, [CallerMemberName] string? callerName = null)
        {
            return new ArgumentException($"{callerName}: Child of type {type} is not supported.");
        }

        public IFluentElement AddVariant()
        {
            var variant = new FluentVariant();

            while(Types.Peek() != FluentTypes.Variant)
            {

            }

            string type;
            do
            {
                type = Types.Pop();
                var element = Elements.Pop();
                switch (element)
                {
                    IFluentContent expression:
                        variant.Content.Add(expression);
                        break;
                    default: break;
                }
            }
            while (type != FluentTypes.Variant);

            var id = result.OfType<FluentIdentifier>().FirstOrDefault();
            if (id != null)
            {
                result.Remove(id);
                variant.Identifier = id;
            }

            var numberLiteral = context.NUMBER_LITERAL()?.GetText();
            if (numberLiteral != null)
                variant.Identifier = new FluentNumberLiteral { Value = numberLiteral };

            AggregateContainer(variant, result);

            if (result.Count != 0)
                throw UnsupportedChildTypeException(result[0]);

            result.Add(variant);
        }
    }
}
