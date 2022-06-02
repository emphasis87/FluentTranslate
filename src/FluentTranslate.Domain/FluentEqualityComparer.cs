using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentTranslate.Domain;
using static FluentTranslate.Infrastructure.EqualityHelper;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentEqualityComparer : IEqualityComparer<IFluentElement>
	{

	}

	public class FluentEqualityComparer : IFluentEqualityComparer
	{
		public static IFluentEqualityComparer Default { get; } = new FluentEqualityComparer();

		public int GetHashCode(IFluentElement element)
		{
			return HashElement(element);
		}

		public int HashElement(IFluentElement element)
		{
			var hashCode = element switch
			{
				null => 0,
				FluentResource resource => GetHashCode(resource),
				FluentComment comment => GetHashCode(comment),
				FluentMessage message => GetHashCode(message),
				FluentTerm term => GetHashCode(term),
				FluentAttribute attribute => GetHashCode(attribute),
				FluentText text => GetHashCode(text),
				FluentPlaceable placeable => GetHashCode(placeable),
				FluentSelection selection => GetHashCode(selection),
				FluentVariant variant => GetHashCode(variant),
				FluentFunctionCall functionCall => GetHashCode(functionCall),
				FluentCallArgument argument => GetHashCode(argument),
				FluentIdentifier identifier => GetHashCode(identifier),
				FluentMessageReference messageReference => GetHashCode(messageReference),
				FluentTermReference termReference => GetHashCode(termReference),
				FluentVariableReference variableReference => GetHashCode(variableReference),
				FluentNumberLiteral numberLiteral => GetHashCode(numberLiteral),
				FluentStringLiteral stringLiteral => GetHashCode(stringLiteral),
				_ => throw new ArgumentOutOfRangeException(nameof(element))
			};
			return hashCode;
		}

		public int GetHashCode(FluentResource resource)
		{
			return Hash(resource.Entries);
		}

		public int GetHashCode(FluentComment comment)
		{
			return Hash(comment.Value);
		}

		public int GetHashCode(FluentMessage message)
		{
			return Hash(message.Reference);
		}

		public int GetHashCode(FluentTerm term)
		{
			return Hash(term.Reference);
		}

		public int GetHashCode(FluentAttribute attribute)
		{
			return Hash(attribute.Id);
		}

		public int GetHashCode(FluentText text)
		{
			return Hash(text.Value);
		}

		public int GetHashCode(FluentPlaceable placeable)
		{
			return Combine(placeable.Type, placeable.Content);
		}

		public int GetHashCode(FluentSelection selection)
		{
			return Combine(selection.Type, selection.Match, selection.Variants);
		}

		public int GetHashCode(FluentVariant variant)
		{
			return Combine(variant.Type, variant.Key);
		}

		public int GetHashCode(FluentFunctionCall functionCall)
		{
			return Hash(functionCall.Id);
		}

		public int GetHashCode(FluentCallArgument argument)
		{
			return Hash(argument.Id);
		}

		public int GetHashCode(FluentIdentifier identifier)
		{
			return Hash(identifier.Id);
		}

		public int GetHashCode(FluentMessageReference messageReference)
		{
			return Hash(messageReference.Reference);
		}

		public int GetHashCode(FluentTermReference termReference)
		{
			return Hash(termReference.Reference);
		}

		public int GetHashCode(FluentVariableReference variableReference)
		{
			return Hash(variableReference.Id);
		}

		public int GetHashCode(FluentNumberLiteral numberLiteral)
		{
			return Hash(numberLiteral.Value);
		}

		public int GetHashCode(FluentStringLiteral stringLiteral)
		{
			return Hash(stringLiteral.Value);
		}

		public bool Equals(IFluentElement element1, IFluentElement element2)
		{
			if (element1 is null || element2 is null) return false;
			if (ReferenceEquals(element1, element2)) return true;
			if (element1.GetType() != element2.GetType()) return false;
			var result = (element1, element2) switch
			{
				(FluentResource x, FluentResource y) => Equals(x, y),
				(FluentComment x, FluentComment y) => Equals(x, y),
				(FluentMessage x, FluentMessage y) => Equals(x, y),
				(FluentTerm x, FluentTerm y) => Equals(x, y),
				(FluentAttribute x, FluentAttribute y) => Equals(x, y),
				(FluentText x, FluentText y) => Equals(x, y),
				(FluentPlaceable x, FluentPlaceable y) => Equals(x, y),
				(FluentSelection x, FluentSelection y) => Equals(x, y),
				(FluentVariant x, FluentVariant y) => Equals(x, y),
				(FluentFunctionCall x, FluentFunctionCall y) => Equals(x, y),
				(FluentCallArgument x, FluentCallArgument y) => Equals(x, y),
				(FluentIdentifier x, FluentIdentifier y) => Equals(x, y),
				(FluentMessageReference x, FluentMessageReference y) => Equals(x, y),
				(FluentTermReference x, FluentTermReference y) => Equals(x, y),
				(FluentVariableReference x, FluentVariableReference y) => Equals(x, y),
				(FluentNumberLiteral x, FluentNumberLiteral y) => Equals(x, y),
				(FluentStringLiteral x, FluentStringLiteral y) => Equals(x, y),
				_ => throw new ArgumentOutOfRangeException(nameof(element1))
			};
			return result;
		}

		public bool Equals(FluentResource x, FluentResource y)
		{
			return AreEqual(x.Entries, y.Entries);
		}

		public bool Equals(FluentComment x, FluentComment y)
		{
			return x.Level == y.Level
				&& x.Value == y.Value;
		}

		public bool Equals(FluentMessage x, FluentMessage y)
		{
			return x.Reference == y.Reference
				&& x.Comment == y.Comment
				&& AreEqual(x.Attributes, y.Attributes)
				&& AreEqual(x.Content, y.Content);
		}

		public bool Equals(FluentTerm x, FluentTerm y)
		{
			return x.Reference == y.Reference
				&& x.Comment == y.Comment
				&& AreEqual(x.Attributes, y.Attributes)
				&& AreEqual(x.Content, y.Content);
		}

		public bool Equals(FluentAttribute x, FluentAttribute y)
		{
			return x.Id == y.Id
				&& AreEqual(x.Content, y.Content);
		}

		public bool Equals(FluentText x, FluentText y)
		{
			return x.Value == y.Value;
		}

		public bool Equals(FluentPlaceable x, FluentPlaceable y)
		{
			return AreEqual(x.Content, y.Content);
		}

		public bool Equals(FluentSelection x, FluentSelection y)
		{
			return AreEqual(x.Match, y.Match)
				&& AreEqual(x.Variants, y.Variants);
		}

		public bool Equals(FluentVariant x, FluentVariant y)
		{
			return x.IsDefault == y.IsDefault
				&& AreEqual(x.Key, y.Key)
				&& AreEqual(x.Content, y.Content);
		}

		public bool Equals(FluentFunctionCall x, FluentFunctionCall y)
		{
			return x.Id == y.Id
				&& AreEqual(x.Arguments, y.Arguments);
		}

		public bool Equals(FluentCallArgument x, FluentCallArgument y)
		{
			return x.Id == y.Id
				&& AreEqual(x.Value, y.Value);
		}

		public bool Equals(FluentIdentifier x, FluentIdentifier y)
		{
			return x.Id == y.Id;
		}

		public bool Equals(FluentMessageReference x, FluentMessageReference y)
		{
			return x.Reference == y.Reference;
		}

		public bool Equals(FluentTermReference x, FluentTermReference y)
		{
			return x.Reference == y.Reference
				&& AreEqual(x.Arguments, y.Arguments);
		}

		public bool Equals(FluentVariableReference x, FluentVariableReference y)
		{
			return x.Id == y.Id;
		}

		public bool Equals(FluentNumberLiteral x, FluentNumberLiteral y)
		{
			return x.Value == y.Value;
		}

		public bool Equals(FluentStringLiteral x, FluentStringLiteral y)
		{
			return x.Value == y.Value;
		}
	}

	internal class EqualityHelper
	{
		public static int Hash(object item)
		{
			return item switch
			{
				null => 0,
				IFluentElement element => element.GetHashCode(),
				IEnumerable list => Combine(list.Cast<object>().ToArray()),
				{ } other => other.GetHashCode(),
			};
		}

		public static int Combine(params object[] items)
		{
			unchecked
			{
				var hash = 17;
				foreach (var item in items)
				{
					hash = hash * 31 + Hash(item);
				}

				return hash;
			}
		}

		public static bool AreEqual(IEnumerable x, IEnumerable y)
		{
			if (x is null || y is null) return false;
			var l1 = x.Cast<object>().ToArray();
			var l2 = y.Cast<object>().ToArray();
			return l1.Length == l2.Length
				&& l1.Zip(l2, AreEqual).All(r => r);
		}

		public static bool AreEqual(object x, object y)
		{
			if (x is null || y is null) return false;
			if (ReferenceEquals(x, y)) return true;
			return x.Equals(y);
		}
	}
}
