using FluentTranslate.Domain;

namespace FluentTranslate.Common
{
    public static class FluentTypes
    {
        public const string Resource = "resource";
        public const string EmptyLines = "empty-lines";
        public const string Comment = "comment";
        public const string Message = "message";
        public const string Term = "term";
        public const string Attribute = "attribute";
        public const string Text = "text";
        public const string Placeable = "placeable";
        public const string Selection = "selection";
        public const string Variant = "variant";
        public const string Identifier = "id";
        public const string MessageReference = "message-ref";
        public const string TermReference = "term-ref";
        public const string VariableReference = "variable-ref";
        public const string FunctionCall = "function-call";
        public const string CallArgument = "argument";
        public const string NumberLiteral = "number";
        public const string StringLiteral = "string";

        public static string GetType(IFluentElement element)
        {
            return element switch
            {
                FluentResource => Resource,
                FluentEmptyLines => EmptyLines,
                FluentComment => Comment,
                FluentMessage => Message,
                FluentTerm => Term,
                FluentAttribute => Attribute,
                FluentText => Text,
                FluentPlaceable => Placeable,
                FluentSelection => Selection,
                FluentVariant => Variant,
                FluentFunctionCall => FunctionCall,
                FluentCallArgument => CallArgument,
                FluentIdentifier => Identifier,
                FluentMessageReference => MessageReference,
                FluentTermReference => TermReference,
                FluentVariableReference => VariableReference,
                FluentNumberLiteral => NumberLiteral,
                FluentStringLiteral => StringLiteral,
                _ => throw new ArgumentOutOfRangeException(nameof(element))
            };
        }
    }
}
