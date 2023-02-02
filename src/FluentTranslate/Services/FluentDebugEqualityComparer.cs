using FluentTranslate.Domain;

using static FluentTranslate.Common.Helpers;

namespace FluentTranslate.Services
{
    public class FluentDebugEqualityComparer : IFluentEqualityComparer
    {
        private Stack<string> _path = new();

        public bool Equals(IFluentElement? x, IFluentElement? y)
        {
            CheckReferences(x, y);

            _path.Push($"/{x.Type}");

            var result = (x, y) switch
            {
                (FluentDocument a, FluentDocument b) => Equals(a, b),
                (FluentEmptyLines a, FluentComment b) => Equals(a, b),
                (FluentComment a, FluentComment b) => Equals(a, b),
                (FluentTerm a, FluentTerm b) => Equals(a, b),
                (FluentMessage a, FluentMessage b) => Equals(a, b),
                (FluentAttribute a, FluentAttribute b) => Equals(a, b),
                (FluentText a, FluentText b) => Equals(a, b),
                (FluentPlaceable a, FluentPlaceable b) => Equals(a, b),
                (FluentSelection a, FluentSelection b) => Equals(a, b),
                (FluentVariant a, FluentVariant b) => Equals(a, b),
                (FluentFunctionCall a, FluentFunctionCall b) => Equals(a, b),
                (FluentCallArgument a, FluentCallArgument b) => Equals(a, b),
                (FluentIdentifier a, FluentIdentifier b) => Equals(a, b),
                (FluentMessageReference a, FluentMessageReference b) => Equals(a, b),
                (FluentTermReference a, FluentTermReference b) => Equals(a, b),
                (FluentVariableReference a, FluentVariableReference b) => Equals(a, b),
                (FluentNumberLiteral a, FluentNumberLiteral b) => Equals(a, b),
                (FluentStringLiteral a, FluentStringLiteral b) => Equals(a, b),
                (IFluentElement a, IFluentElement b) => throw new ArgumentException(Error($"Unexpected types for Equals: {a.GetType()}, {b.GetType()}"))
            };

            _path.Pop();

            return result;
        }

        protected bool CheckReferences(object? x, object? y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x is null)
                throw new ArgumentException(Error($"Expected type: null", $"Actual type: {y.GetType()}"));
            if (y is null)
                throw new ArgumentException(Error($"Expected type: {x.GetType()}", $"Actual type: null"));
            if (x.GetType() != y.GetType())
                throw new ArgumentException(Error($"Expected type: {x.GetType()}", $"Actual type: {y.GetType()}"));

            return true;
        }

        protected string Error(params string[] message)
        {
            var path = string.Join("", _path.Reverse());
            _path.Clear();
            return string.Join("\r\n\t", message.Prepend(path));
        }

        protected bool SequenceEquals(IEnumerable<IFluentElement> x, IEnumerable<IFluentElement> y)
        {
            return ZipOrDefault(x, y, (a, b) => (a, b))
                .Select((p, i) => PropertyEquals($"[{i}]", () => Equals(p.a, p.b)))
                .All(e => e);
        }

        protected bool PropertyEquals<T>(string path, T? a, T? b)
            where T : IEquatable<T>
        {
            return PropertyEquals(path, () =>
            {
                var result = Equals(a, b);
                if (!result)
                    throw new ArgumentException(Error($"Expected value: '{a}'", $"Actual value: '{b}'"));
                return result;
            });
        }

        protected bool PropertyEquals(string path, Func<bool> equals)
        {
            if (string.IsNullOrEmpty(path) || path.StartsWith("["))
                _path.Push(path);
            else
                _path.Push($".{path}");
            var result = equals();
            _path.Pop();
            return result;
        }

        protected bool Equals(FluentDocument x, FluentDocument y)
        {
            return SequenceEquals(x.Content, y.Content);
        }

        protected bool Equals(FluentEmptyLines x, FluentEmptyLines y)
        {
            return ReferenceEquals(x, y);
        }

        protected bool Equals(FluentComment x, FluentComment y)
        {
            return PropertyEquals(nameof(x.Level), x.Level, y.Level)
                && PropertyEquals(nameof(x.Value), x.Value, y.Value);
        }

        protected bool Equals(FluentMessage x, FluentMessage y)
        {
            return PropertyEquals(nameof(x.Reference), x.Reference, y.Reference)
                && PropertyEquals(nameof(x.Comment), x.Comment, y.Comment)
                && PropertyEquals(nameof(x.Content), () => SequenceEquals(x.Content, y.Content))
                && PropertyEquals(nameof(x.Attributes), () => SequenceEquals(x.Attributes, y.Attributes));
        }

        public bool Equals(FluentTerm x, FluentTerm y)
        {
            return PropertyEquals(nameof(x.Reference), x.Reference, y.Reference)
                && PropertyEquals(nameof(x.Comment), x.Comment, y.Comment)
                && PropertyEquals(nameof(x.Content), () => SequenceEquals(x.Content, y.Content))
                && PropertyEquals(nameof(x.Attributes), () => SequenceEquals(x.Attributes, y.Attributes));
        }

        public bool Equals(FluentAttribute x, FluentAttribute y)
        {
            return PropertyEquals(nameof(x.Id), x.Id, y.Id)
                && PropertyEquals(nameof(x.Content), () => SequenceEquals(x.Content, y.Content));
        }

        public bool Equals(FluentText x, FluentText y)
        {
            return PropertyEquals(nameof(x.Value), x.Value, y.Value);
        }

        public bool Equals(FluentPlaceable x, FluentPlaceable y)
        {
            return PropertyEquals(nameof(x.Content), () => Equals(x.Content, y.Content));
        }

        public bool Equals(FluentSelection x, FluentSelection y)
        {
            return PropertyEquals(nameof(x.Match), () => Equals(x.Match, y.Match))
                && PropertyEquals(nameof(x.Variants), () => SequenceEquals(x.Variants, y.Variants));
        }

        public bool Equals(FluentVariant x, FluentVariant y, IEqualityComparer<IFluentElement>? comparer = null)
        {
            return PropertyEquals(nameof(x.IsDefault), x.IsDefault, y.IsDefault)
                && PropertyEquals(nameof(x.Identifier), () => Equals(x.Identifier, y.Identifier))
                && PropertyEquals(nameof(x.Content), () => SequenceEquals(x.Content, y.Content));
        }

        public bool Equals(FluentFunctionCall x, FluentFunctionCall y, IEqualityComparer<IFluentElement>? comparer = null)
        {
            return PropertyEquals(nameof(x.Id), x.Id, y.Id)
                && PropertyEquals(nameof(x.Arguments), () => SequenceEquals(x.Arguments, y.Arguments));
        }

        public bool Equals(FluentCallArgument x, FluentCallArgument y, IEqualityComparer<IFluentElement>? comparer = null)
        {
            return PropertyEquals(nameof(x.Identifier), x.Identifier, y.Identifier)
                && PropertyEquals(nameof(x.Content), () => Equals(x.Content, y.Content));
        }

        public bool Equals(FluentIdentifier x, FluentIdentifier y)
        {
            return PropertyEquals(nameof(x.Value), x.Value, y.Value);
        }

        public bool Equals(FluentMessageReference x, FluentMessageReference y)
        {
            return PropertyEquals(nameof(x.Target), x.Target, y.Target);
        }

        public bool Equals(FluentTermReference x, FluentTermReference y, IEqualityComparer<IFluentElement>? comparer = null)
        {
            return PropertyEquals(nameof(x.Target), x.Target, y.Target)
                && PropertyEquals(nameof(x.Arguments), () => SequenceEquals(x.Arguments, y.Arguments));
        }

        public bool Equals(FluentVariableReference x, FluentVariableReference y)
        {
            return PropertyEquals(nameof(x.Id), x.Id, y.Id);
        }

        public bool Equals(FluentNumberLiteral x, FluentNumberLiteral y)
        {
            return PropertyEquals(nameof(x.Value), x.Value, y.Value);
        }

        public bool Equals(FluentStringLiteral x, FluentStringLiteral y)
        {
            return PropertyEquals(nameof(x.Value), x.Value, y.Value);
        }

        public int GetHashCode(IFluentElement element)
        {
            var hashCode = element switch
            {
                null => 0,
                FluentDocument resource => GetHashCode(resource),
                FluentEmptyLines emptyLines => GetHashCode(emptyLines),
                FluentComment comment => GetHashCode(comment),
                FluentTerm term => GetHashCode(term),
                FluentMessage message => GetHashCode(message),
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

        public int GetHashCode(FluentDocument resource) => GetHashCode(resource.Content);
        public int GetHashCode(FluentEmptyLines emptyLines) => emptyLines.Count;
        public int GetHashCode(FluentComment comment) => GetHashCode(comment.Value);
        public int GetHashCode(FluentMessage message) => GetHashCode(message.Reference);
        public int GetHashCode(FluentTerm term) => GetHashCode(term.Reference);
        public int GetHashCode(FluentAttribute attribute) => GetHashCode(attribute.Id);
        public int GetHashCode(FluentText text) => GetHashCode(text.Value);
        public int GetHashCode(FluentPlaceable placeable) => GetHashCode(placeable.Type, placeable.Content);
        public int GetHashCode(FluentSelection selection) => GetHashCode(selection.Type, selection.Match, selection.Variants);
        public int GetHashCode(FluentVariant variant) => GetHashCode(variant.Type, variant.Identifier);
        public int GetHashCode(FluentFunctionCall functionCall) => GetHashCode(functionCall.Id);
        public int GetHashCode(FluentCallArgument argument) => GetHashCode(argument.Identifier);
        public int GetHashCode(FluentIdentifier identifier) => GetHashCode(identifier.Value);
        public int GetHashCode(FluentMessageReference messageReference) => GetHashCode(messageReference.Target);
        public int GetHashCode(FluentTermReference termReference) => GetHashCode(termReference.Target);
        public int GetHashCode(FluentVariableReference variableReference) => GetHashCode(variableReference.Id);
        public int GetHashCode(FluentNumberLiteral numberLiteral) => GetHashCode(numberLiteral.Value);
        public int GetHashCode(FluentStringLiteral stringLiteral) => GetHashCode(stringLiteral.Value);

        protected virtual int GetHashCode(params object?[] value)
        {
            return Hash(value);
        }
    }
}
