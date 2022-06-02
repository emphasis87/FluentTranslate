using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentTranslate.Domain;

namespace FluentTranslate.Serialization
{
    public interface IFluentSerializer
    {
        string Serialize(IFluentElement element, FluentSerializerContext context = null);
    }

    /// <summary>
    /// Serializer from <see cref="IFluentElement"/> to fluent syntax.
    /// </summary>
    public class FluentSerializer : IFluentSerializer
    {
        public static FluentSerializer Default { get; } = new FluentSerializer();

        protected virtual FluentSerializerContext CreateContext() => new FluentSerializerContext();

        public virtual string Serialize(IFluentElement element, FluentSerializerContext context = null)
        {
			context ??= CreateContext();
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

        public virtual string Serialize(FluentResource resource, FluentSerializerContext context = null)
		{
			context ??= CreateContext();
            var entryItems = resource.Entries
                .Select(entry => Serialize(entry, context))
                .ToList();
			var entries = string.Join("\r\n", entryItems);
			return entries;
        }

        public virtual string Serialize(FluentEmptyLines emptyLines, FluentSerializerContext context = null)
        {
            return string.Join("", Enumerable.Repeat("\r\n", Math.Max(1, emptyLines.Count)));
        }

        public virtual string Serialize(FluentComment comment, FluentSerializerContext context = null)
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

        public virtual string Serialize(FluentMessage message, FluentSerializerContext context = null)
        {
			context ??= CreateContext();
            var comment = string.IsNullOrWhiteSpace(message.Comment) ? null : Serialize(new FluentComment(1, message.Comment), context);
            var entry = $"{message.Reference} = ";
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

        public virtual string Serialize(FluentTerm term, FluentSerializerContext context = null)
        {
			context ??= CreateContext();
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

        public virtual string Serialize(FluentAttribute attribute, FluentSerializerContext context = null)
        {
			context ??= CreateContext();
            var entry = $"{context.Indent}.{attribute.Identifier} =";
            context.AddIndent();
            var contents = attribute.Content
                .Select(content => Serialize(content, context))
                .ToList();
            var entryIndent = contents.Contains("\r\n") ? "\r\n" : " ";
            context.RemoveIndent();
			context.IsTextContinuation = false;
            return $"{entry}{entryIndent}{string.Join("", contents)}\r\n";
        }

        public virtual string Serialize(FluentText text, FluentSerializerContext context = null)
		{
			context ??= CreateContext();
            var lines = text.Value.Split(new[] {"\r\n"}, StringSplitOptions.None);
			var result = string.Join($"\r\n{context.Indent}", lines);
			context.IsTextContinuation = true;
            return $"{result}";
        }

        public virtual string Serialize(FluentPlaceable placeable, FluentSerializerContext context = null)
		{
			context ??= CreateContext();
            var content = Serialize(placeable.Content, context);
			var isMultiline = content.Contains("\r\n");
            var closingIndent = isMultiline ? $"{context.Indent}" : " ";
			if (isMultiline)
				context.IsTextContinuation = true;
			return $"{{ {content}{closingIndent}}}";
		}

        public virtual string Serialize(FluentSelection selection, FluentSerializerContext context = null)
		{
			context ??= CreateContext();
            var match = Serialize(selection.Match, context);
            context.AddIndent();
			var variantItems = selection.Variants
				.Select(variant => Serialize(variant, context))
				.ToList();
            context.RemoveIndent();
			var variants = string.Join("", variantItems);
			return $"{match} ->\r\n{variants}";
		}

        public virtual string Serialize(FluentVariant variant, FluentSerializerContext context = null)
		{
			context ??= CreateContext();
            var isDefault = variant.IsDefault ? "*" : " ";
			var key = Serialize(variant.Identifier, context);
			var contentItems = variant.Content
				.Select(content => Serialize(content, context))
				.ToList();
			var contents = string.Join("", contentItems);
			return $"{context.Indent}{isDefault}[{key}] {contents}\r\n";
		}

        public virtual string Serialize(FluentVariableReference variableReference, FluentSerializerContext context = null)
		{
            return $"${variableReference.TargetId}";
		}

        public virtual string Serialize(FluentMessageReference messageReference, FluentSerializerContext context = null)
		{
            return messageReference.TargetReference;
		}

        public virtual string Serialize(FluentTermReference termReference, FluentSerializerContext context = null)
		{
			context ??= CreateContext();
            var reference = termReference.TargetReference;
			var call = Serialize(new FluentFunctionCall {Arguments = termReference.Arguments.ToList()}, context);
			return $"{reference}{call}";
		}

        public virtual string Serialize(FluentFunctionCall functionCall, FluentSerializerContext context = null)
		{
			context ??= CreateContext();
            var arguments = functionCall.Arguments
				.Select(argument => Serialize(argument, context))
				.ToList();
			return $"{functionCall.TargetId}({string.Join(", ", arguments)})";
		}

        public virtual string Serialize(FluentCallArgument callArgument, FluentSerializerContext context = null)
		{
			context ??= CreateContext();
            var name = string.IsNullOrWhiteSpace(callArgument.Identifier) ? null : $"{callArgument.Identifier}: ";
			var value = Serialize(callArgument.Value, context);
			return $"{name}{value}";
		}

        public virtual string Serialize(FluentStringLiteral stringLiteral, FluentSerializerContext context = null)
		{
            return $"\"{stringLiteral.Value}\"";
		}

        public virtual string Serialize(FluentNumberLiteral numberLiteral, FluentSerializerContext context = null)
		{
            return numberLiteral.Value;
		}

        public virtual string Serialize(FluentIdentifier identifier, FluentSerializerContext context = null)
		{
            return identifier.Value;
		}
    }
}
