using System;
using System.Linq;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
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
			var clone = new FluentResource
			{
				Entries = resource.Entries.Select(Clone).ToList()
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
				Id = message.Id,
				Comment = message.Comment,
				Content = message.Content.Select(Clone).ToList(),
				Attributes = message.Attributes.Select(Clone).ToList(),
			};
			return clone;
		}

		public FluentTerm Clone(FluentTerm term)
		{
			var clone = new FluentTerm
			{
				Id = term.Id,
				Comment = term.Comment,
				Content = term.Content.Select(Clone).ToList(),
				Attributes = term.Attributes.Select(Clone).ToList(),
			};
			return clone;
		}

		public FluentAttribute Clone(FluentAttribute attribute)
		{
			var clone = new FluentAttribute
			{
				Id = attribute.Id,
				Content = attribute.Content.Select(Clone).ToList()
			};
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
				Variants = selection.Variants.Select(Clone).ToList(),
			};
			return clone;
		}

		public FluentVariant Clone(FluentVariant variant)
		{
			var clone = new FluentVariant
			{
				IsDefault = variant.IsDefault,
				Key = Clone(variant.Key),
				Content = variant.Content.Select(Clone).ToList(),
			};
			return clone;
		}

		public FluentFunctionCall Clone(FluentFunctionCall functionCall)
		{
			var clone = new FluentFunctionCall
			{
				Id = functionCall.Id,
				Arguments = functionCall.Arguments.Select(Clone).ToList()
			};
			return clone;
		}

		public FluentCallArgument Clone(FluentCallArgument argument)
		{
			var clone = new FluentCallArgument
			{
				Id = argument.Id,
				Value = Clone(argument.Value)
			};
			return clone;
		}

		public FluentIdentifier Clone(FluentIdentifier identifier)
		{
			var clone = new FluentIdentifier
			{
				Id = identifier.Id
			};
			return clone;
		}

		public FluentMessageReference Clone(FluentMessageReference messageReference)
		{
			var clone = new FluentMessageReference
			{
				Id = messageReference.Id,
				AttributeId = messageReference.AttributeId
			};
			return clone;
		}

		public FluentTermReference Clone(FluentTermReference termReference)
		{
			var clone = new FluentTermReference
			{
				Id = termReference.Id,
				AttributeId = termReference.AttributeId,
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