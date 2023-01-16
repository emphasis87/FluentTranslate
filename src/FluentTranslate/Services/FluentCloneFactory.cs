using FluentTranslate.Domain;

namespace FluentTranslate.Services
{
    public interface IFluentCloneFactory
    {
        IFluentElement Clone(IFluentElement element);
        T Clone<T>(T element) where T : IFluentElement;
    }

    /// <summary>
    /// Clones fluent elements.
    /// </summary>
    public class FluentCloneFactory : IFluentCloneFactory
    {
        public static FluentCloneFactory Default { get; } = new FluentCloneFactory();

        public T Clone<T>(T element) where T : IFluentElement
        {
            return (T)Clone((IFluentElement)element);
        }

        public IFluentElement Clone(IFluentElement element)
        {
            IFluentElement clone = element switch
            {
                FluentResource resource => Clone(resource),
                FluentEmptyLines emptyLines => Clone(emptyLines),
                FluentComment comment => Clone(comment),
                FluentMessage message => Clone(message),
                FluentTerm term => Clone(term),
                FluentAttribute attribute => Clone(attribute),
                FluentText text => Clone(text),
                FluentPlaceable placeable => Clone(placeable),
                FluentSelection selection => Clone(selection),
                FluentVariant variant => Clone(variant),
                FluentFunctionCall functionCall => Clone(functionCall),
                FluentCallArgument argument => Clone(argument),
                FluentIdentifier identifier => Clone(identifier),
                FluentMessageReference messageReference => Clone(messageReference),
                FluentTermReference termReference => Clone(termReference),
                FluentVariableReference variableReference => Clone(variableReference),
                FluentNumberLiteral numberLiteral => Clone(numberLiteral),
                FluentStringLiteral stringLiteral => Clone(stringLiteral),
                _ => throw new ArgumentOutOfRangeException(nameof(element))
            };
            return clone;
        }

        public FluentResource Clone(FluentResource resource)
        {
            var clone = new FluentResource();
            clone.Entries.AddRange(resource.Entries.Select(Clone));
            return clone;
        }

        public FluentEmptyLines Clone(FluentEmptyLines emptyLines)
        {
            var clone = new FluentEmptyLines
            {
                Count = emptyLines.Count
            };
            return clone;
        }

        public FluentComment Clone(FluentComment comment)
        {
            var clone = new FluentComment
            {
                Level = comment.Level,
                Value = comment.Value
            };
            return clone;
        }

        public FluentMessage Clone(FluentMessage message)
        {
            var clone = new FluentMessage
            {
                Identifier = message.Identifier,
                Comment = message.Comment,
            };
            clone.Content.AddRange(message.Content.Select(Clone));
            clone.Attributes.AddRange(message.Attributes.Select(Clone));
            return clone;
        }

        public FluentTerm Clone(FluentTerm term)
        {
            var clone = new FluentTerm
            {
                Identifier = term.Identifier,
                Comment = term.Comment,
            };
            clone.Content.AddRange(term.Content.Select(Clone));
            clone.Attributes.AddRange(term.Attributes.Select(Clone));
            return clone;
        }

        public FluentAttribute Clone(FluentAttribute attribute)
        {
            var clone = new FluentAttribute
            {
                Identifier = attribute.Identifier,
            };
            clone.Content.AddRange(attribute.Content.Select(Clone));
            return clone;
        }

        public FluentText Clone(FluentText text)
        {
            var clone = new FluentText
            {
                Value = text.Value
            };
            return clone;
        }

        public FluentPlaceable Clone(FluentPlaceable placeable)
        {
            var clone = new FluentPlaceable
            {
                Content = Clone(placeable.Content)
            };
            return clone;
        }

        public FluentSelection Clone(FluentSelection selection)
        {
            var clone = new FluentSelection
            {
                Match = Clone(selection.Match),
            };
            clone.Variants.AddRange(selection.Variants.Select(Clone));
            return clone;
        }

        public FluentVariant Clone(FluentVariant variant)
        {
            var clone = new FluentVariant
            {
                IsDefault = variant.IsDefault,
                Identifier = Clone(variant.Identifier),
            };
            clone.Content.AddRange(variant.Content.Select(Clone));
            return clone;
        }

        public FluentFunctionCall Clone(FluentFunctionCall functionCall)
        {
            var clone = new FluentFunctionCall
            {
                TargetId = functionCall.TargetId,
                Arguments = functionCall.Arguments.Select(Clone).ToList()
            };
            return clone;
        }

        public FluentCallArgument Clone(FluentCallArgument argument)
        {
            var clone = new FluentCallArgument
            {
                Identifier = argument.Identifier,
                Content = Clone(argument.Content)
            };
            return clone;
        }

        public FluentIdentifier Clone(FluentIdentifier identifier)
        {
            var clone = new FluentIdentifier
            {
                Value = identifier.Value
            };
            return clone;
        }

        public FluentMessageReference Clone(FluentMessageReference messageReference)
        {
            var clone = new FluentMessageReference
            {
                TargetId = messageReference.TargetId,
                TargetAttributeId = messageReference.TargetAttributeId
            };
            return clone;
        }

        public FluentTermReference Clone(FluentTermReference termReference)
        {
            var clone = new FluentTermReference
            {
                TargetId = termReference.TargetId,
                TargetAttributeId = termReference.TargetAttributeId,
                Arguments = termReference.Arguments.Select(Clone).ToList()
            };
            return clone;
        }

        public FluentNumberLiteral Clone(FluentNumberLiteral numberLiteral)
        {
            var clone = new FluentNumberLiteral
            {
                Value = numberLiteral.Value
            };
            return clone;
        }

        public FluentStringLiteral Clone(FluentStringLiteral stringLiteral)
        {
            var clone = new FluentStringLiteral
            {
                Value = stringLiteral.Value
            };
            return clone;
        }
    }
}