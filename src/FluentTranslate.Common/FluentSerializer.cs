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
            var entryItems = resource.Entries
                .Select(entry => Serialize(entry, context))
                .ToList();
			var entries = string.Join("\r\n", entryItems);
			return entries;
        }

        public string Serialize(FluentComment comment, FluentSerializationContext context)
		{
			if (string.IsNullOrWhiteSpace(comment.Value))
				return null;
            var result = new StringBuilder();
            var prefix = $"{new string('#', comment.Level)} ";
			var lines = comment.Value.Split(new[] {"\r\n"}, StringSplitOptions.None);
			foreach (var line in lines)
				result.Append($"{prefix}{line.TrimEnd()}\r\n");
			return $"{result}";
		}

        public string Serialize(FluentMessage message, FluentSerializationContext context)
        {
            var comment = string.IsNullOrWhiteSpace(message.Comment) ? null : Serialize(new FluentComment(1, message.Comment), context);
            var entry = $"{message.Reference} =";
            context.AddIndent();
            var contentItems = message.Content
                .Select(content => Serialize(content, context))
                .ToList();
			var contents = string.Join("", contentItems);
			if (contents.Contains("\r\n"))
				contents = $"\r\n{context.Indent}{contents}";
			context.IsTextContinuation = false;
            var attributes = message.Attributes
                .Select(attribute => Serialize(attribute, context))
                .ToList();
            context.RemoveIndent();
			return $"{comment}{entry}{contents}\r\n{string.Join("", attributes)}";
		}

        public string Serialize(FluentTerm term, FluentSerializationContext context)
        {
            var comment = string.IsNullOrWhiteSpace(term.Comment) ? null : Serialize(new FluentComment(1, term.Comment), context);
            var entry = $"{term.Reference} =";
            context.AddIndent();
            var contentItems = term.Content
                .Select(content => Serialize(content, context))
                .ToList();
			var contents = string.Join("", contentItems);
			if (contents.Contains("\r\n"))
				contents = $"\r\n{contents}";
            context.IsTextContinuation = false;
            var attributes = term.Attributes
                .Select(attribute => Serialize(attribute, context))
                .ToList();
            context.RemoveIndent();
            return $"{comment}{entry}{contents}\r\n{string.Join("", attributes)}";
        }

        public string Serialize(FluentAttribute attribute, FluentSerializationContext context)
        {
            var entry = $"{context.Indent}.{attribute.Id} =";
            context.AddIndent();
            var contents = attribute.Content
                .Select(content => Serialize(content, context))
                .ToList();
            var entryIndent = contents.Contains("\r\n") ? "\r\n" : " ";
            context.RemoveIndent();
			context.IsTextContinuation = false;
            return $"{entry}{entryIndent}{string.Join("", contents)}\r\n";
        }

        public string Serialize(FluentText text, FluentSerializationContext context)
		{
			var lines = text.Value.Split(new[] {"\r\n"}, StringSplitOptions.None);
			var result = string.Join($"\r\n{context.Indent}", lines);
			context.IsTextContinuation = true;
            return $"{result}";
        }

        public string Serialize(FluentPlaceable placeable, FluentSerializationContext context)
		{
			var content = Serialize(placeable.Content, context);
			var isMultiline = content.Contains("\r\n");
            var closingIndent = isMultiline ? $"{context.Indent}" : " ";
			if (isMultiline)
				context.IsTextContinuation = true;
			return $"{{ {content}{closingIndent}}}";
		}

        public string Serialize(FluentSelection selection, FluentSerializationContext context)
		{
			var match = Serialize(selection.Match, context);
            context.AddIndent();
			var variantItems = selection.Variants
				.Select(variant => Serialize(variant, context))
				.ToList();
            context.RemoveIndent();
			var variants = string.Join("", variantItems);
			return $"{match} ->\r\n{variants}";
		}

        public string Serialize(FluentVariant variant, FluentSerializationContext context)
		{
			var isDefault = variant.IsDefault ? "*" : " ";
			var key = Serialize(variant.Key, context);
			var contentItems = variant.Content
				.Select(content => Serialize(content, context))
				.ToList();
			var contents = string.Join("", contentItems);
			return $"{context.Indent}{isDefault}[{key}] {contents}\r\n";
		}

        public string Serialize(FluentVariableReference variableReference, FluentSerializationContext context)
		{
			return $"${variableReference.Id}";
		}

        public string Serialize(FluentMessageReference messageReference, FluentSerializationContext context)
		{
			return messageReference.Reference;
		}

        public string Serialize(FluentTermReference termReference, FluentSerializationContext context)
		{
			var reference = termReference.Reference;
			var call = Serialize(new FluentFunctionCall {Arguments = termReference.Arguments.ToList()}, context);
			return $"{reference}{call}";
		}

        public string Serialize(FluentFunctionCall functionCall, FluentSerializationContext context)
		{
			var arguments = functionCall.Arguments
				.Select(argument => Serialize(argument, context))
				.ToList();
			return $"{functionCall.Id}({string.Join(", ", arguments)})";
		}

        public string Serialize(FluentCallArgument callArgument, FluentSerializationContext context)
		{
			var name = string.IsNullOrWhiteSpace(callArgument.Id) ? null : $"{callArgument.Id}: ";
			var value = Serialize(callArgument.Value, context);
			return $"{name}{value}";
		}

        public string Serialize(FluentStringLiteral stringLiteral, FluentSerializationContext context)
		{
			return $"\"{stringLiteral.Value}\"";
		}

        public string Serialize(FluentNumberLiteral numberLiteral, FluentSerializationContext context)
		{
			return numberLiteral.Value;
		}

        public string Serialize(FluentIdentifier identifier, FluentSerializationContext context)
		{
			return identifier.Id;
		}
    }
}
