﻿using System;
using System.Collections.Generic;

using FluentTranslate.Domain;

using static FluentTranslate.Common.EqualityHelper;

namespace FluentTranslate.Common
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
                FluentEmptyLines emptyLines => GetHashCode(emptyLines),
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

        public static int GetHashCode(FluentResource resource) => Hash((object)resource.Entries);
        public static int GetHashCode(FluentEmptyLines emptyLines) => emptyLines.Count;
        public static int GetHashCode(FluentComment comment) => Hash((object)comment.Value);
        public static int GetHashCode(FluentMessage message) => Hash((object)message.Reference);
        public static int GetHashCode(FluentTerm term) => Hash((object)term.Reference);
        public static int GetHashCode(FluentAttribute attribute) => Hash((object)attribute.Identifier);
        public static int GetHashCode(FluentText text) => Hash((object)text.Value);
        public static int GetHashCode(FluentPlaceable placeable) => Hash(placeable.Type, placeable.Content);
        public static int GetHashCode(FluentSelection selection) => Hash(selection.Type, selection.Match, selection.Variants);
        public static int GetHashCode(FluentVariant variant) => Hash(variant.Type, variant.Identifier);
        public static int GetHashCode(FluentFunctionCall functionCall) => Hash((object)functionCall.TargetId);
        public static int GetHashCode(FluentCallArgument argument) => Hash((object)argument.Identifier);
        public static int GetHashCode(FluentIdentifier identifier) => Hash((object)identifier.Value);
        public static int GetHashCode(FluentMessageReference messageReference) => Hash((object)messageReference.TargetReference);
        public static int GetHashCode(FluentTermReference termReference) => Hash((object)termReference.TargetReference);
        public static int GetHashCode(FluentVariableReference variableReference) => Hash((object)variableReference.TargetId);
        public static int GetHashCode(FluentNumberLiteral numberLiteral) => Hash((object)numberLiteral.Value);
        public static int GetHashCode(FluentStringLiteral stringLiteral) => Hash((object)stringLiteral.Value);

        public bool Equals(IFluentElement e1, IFluentElement e2)
        {
            if (e1 is null || e2 is null) return false;
            if (ReferenceEquals(e1, e2)) return true;
            if (e1.GetType() != e2.GetType()) return false;
            var result = (e1, e2) switch
            {
                (FluentResource x, FluentResource y) => Equals(x, y),
                (FluentEmptyLines x, FluentComment y) => Equals(x, y),
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
                _ => throw new ArgumentOutOfRangeException(nameof(e1))
            };
            return result;
        }

        public static bool Equals(FluentResource x, FluentResource y)
        {
            return AreEqual(x.Entries, y.Entries);
        }

        public static bool Equals(FluentEmptyLines x, FluentEmptyLines y)
        {
            return ReferenceEquals(x, y);
        }

        public static bool Equals(FluentComment x, FluentComment y)
        {
            return x.Level == y.Level
                && x.Value == y.Value;
        }

        public static bool Equals(FluentMessage x, FluentMessage y)
        {
            return x.Reference == y.Reference
                && x.Comment == y.Comment
                && AreEqual(x.Attributes, y.Attributes)
                && AreEqual(x.Content, y.Content);
        }

        public static bool Equals(FluentTerm x, FluentTerm y)
        {
            return x.Reference == y.Reference
                && x.Comment == y.Comment
                && AreEqual(x.Attributes, y.Attributes)
                && AreEqual(x.Content, y.Content);
        }

        public static bool Equals(FluentAttribute x, FluentAttribute y)
        {
            return x.Identifier == y.Identifier
                && AreEqual(x.Content, y.Content);
        }

        public static bool Equals(FluentText x, FluentText y)
        {
            return x.Value == y.Value;
        }

        public static bool Equals(FluentPlaceable x, FluentPlaceable y)
        {
            return AreEqual(x.Content, y.Content);
        }

        public static bool Equals(FluentSelection x, FluentSelection y)
        {
            return AreEqual(x.Match, y.Match)
                && AreEqual(x.Variants, y.Variants);
        }

        public static bool Equals(FluentVariant x, FluentVariant y)
        {
            return x.IsDefault == y.IsDefault
                && AreEqual(x.Identifier, y.Identifier)
                && AreEqual(x.Content, y.Content);
        }

        public static bool Equals(FluentFunctionCall x, FluentFunctionCall y)
        {
            return x.TargetId == y.TargetId
                && AreEqual(x.Arguments, y.Arguments);
        }

        public static bool Equals(FluentCallArgument x, FluentCallArgument y)
        {
            return x.Identifier == y.Identifier
                && AreEqual(x.Value, y.Value);
        }

        public static bool Equals(FluentIdentifier x, FluentIdentifier y)
        {
            return x.Value == y.Value;
        }

        public static bool Equals(FluentMessageReference x, FluentMessageReference y)
        {
            return x.TargetReference == y.TargetReference;
        }

        public static bool Equals(FluentTermReference x, FluentTermReference y)
        {
            return x.TargetReference == y.TargetReference
                && AreEqual(x.Arguments, y.Arguments);
        }

        public static bool Equals(FluentVariableReference x, FluentVariableReference y)
        {
            return x.TargetId == y.TargetId;
        }

        public static bool Equals(FluentNumberLiteral x, FluentNumberLiteral y)
        {
            return x.Value == y.Value;
        }

        public static bool Equals(FluentStringLiteral x, FluentStringLiteral y)
        {
            return x.Value == y.Value;
        }
    }
}
