using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Common
{
    public interface IFluentSerializer
    {
        string Serialize(IFluentElement element, FluentSerializationContext context);
    }

    public class FluentSerializer : IFluentSerializer
    {
        public string Serialize(IFluentElement element, FluentSerializationContext context)
        {
            context ??= new FluentSerializationContext();
            return element switch
            {
                FluentResource resource => Serialize(resource, context),
                FluentComment comment => Serialize(comment, context),
                FluentMessage message => Serialize(message, context),
                FluentTerm term => Serialize(term, context),
                FluentAttribute attribute => Serialize(attribute, context),
                FluentText text => Serialize(text, context),
                FluentPlaceable placeable => Serialize(placeable, context),
                FluentSelection selection => Serialize(selection, context),
                FluentVariant variant => Serialize(variant, context),
                FluentVariableReference variableReference => Serialize(variableReference, context),
                FluentMessageReference messageReference => Serialize(messageReference, context),
                FluentTermReference termReference => Serialize(termReference, context),
                FluentFunctionCall functionCall => Serialize(functionCall, context),
                FluentCallArgument callArgument => Serialize(callArgument, context),
                FluentStringLiteral stringLiteral => Serialize(stringLiteral, context),
                FluentNumberLiteral numberLiteral => Serialize(numberLiteral, context),
                FluentIdentifier identifier => Serialize(identifier, context),
                _ => throw new ArgumentOutOfRangeException(nameof(element))
            };
        }

        public string Serialize(FluentResource resource, FluentSerializationContext context)
        {
            var entries = resource.Entries
                .Select(entry => Serialize(entry, context))
                .ToList();

            return string.Join("", entries);
        }

        public string Serialize(FluentComment comment, FluentSerializationContext context)
        {
            var prefix = new string('#', comment.Level);
            return $"{prefix} {comment.Value?.Trim()}\r\n";
        }

        public string Serialize(FluentMessage message, FluentSerializationContext context)
        {
            var comment = string.IsNullOrWhiteSpace(message.Comment)
                ? ""
                : Serialize(new FluentComment(1, message.Comment), context);
            var entry = $"{message.Reference} =";
            var contents = message.Content
                .Select(content => Serialize(content, context))
                .ToList();
            var entryIndent = contents.Contains("\r\n") ? "\r\n" : " ";
            var attributes = message.Attributes
                .Select(attribute => Serialize(attribute, context))
                .ToList();
            return $"{comment}{entry}{entryIndent}{contents}{attributes}";
        }

        public string Serialize(FluentTerm term, FluentSerializationContext context)
        {
            var comment = string.IsNullOrWhiteSpace(term.Comment)
                ? ""
                : Serialize(new FluentComment(1, term.Comment), context);
            var entry = $"{term.Reference} =";
            var contents = term.Content
                .Select(content => Serialize(content, context))
                .ToList();
            var entryIndent = contents.Contains("\r\n") ? "\r\n" : " ";
            var attributes = term.Attributes
                .Select(attribute => Serialize(attribute, context))
                .ToList();
            return $"{comment}{entry}{entryIndent}{contents}{attributes}";
        }

        public string Serialize(FluentAttribute attribute, FluentSerializationContext context)
        {
            var entry = $".{attribute.Id} =";
            var contents = attribute.Content
                .Select(content => Serialize(content, context))
                .ToList();
            var entryIndent = contents.Contains("\r\n") ? "\r\n" : " ";
            return $"{entry}{entryIndent}{contents}";
        }

        public string Serialize(FluentText text, FluentSerializationContext context)
        {
            return $"{text.Value}";
        }

        public string Serialize(FluentPlaceable placeable, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }

        public string Serialize(FluentSelection selection, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }

        public string Serialize(FluentVariant variant, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }

        public string Serialize(FluentVariableReference variableReference, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }

        public string Serialize(FluentMessageReference messageReference, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }

        public string Serialize(FluentTermReference termReference, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }

        public string Serialize(FluentFunctionCall functionCall, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }

        public string Serialize(FluentCallArgument callArgument, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }

        public string Serialize(FluentStringLiteral stringLiteral, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }

        public string Serialize(FluentNumberLiteral numberLiteral, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }

        public string Serialize(FluentIdentifier identifier, FluentSerializationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
