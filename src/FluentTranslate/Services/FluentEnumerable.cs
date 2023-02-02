using FluentTranslate.Domain;

namespace FluentTranslate.Services
{
    public class FluentEnumerable
    {
        public static bool CanEnumerate(IFluentElement item)
        {
            switch (item)
            {
                case FluentDocument:
                case FluentTerm:
                case FluentMessage:
                case FluentAttribute:
                case FluentPlaceable:
                case FluentSelection:
                case FluentVariant:
                case FluentFunctionCall:
                case FluentTermReference:
                case FluentCallArgument:
                    return true;
            }

            return false;
        }

        public static IEnumerable<IFluentElement> Enumerate(IFluentElement item)
        {
            if (item is null)
                yield break;

            switch (item)
            {
                case FluentDocument document: 
                    foreach(var _ in document.Content) yield return _;
                    break;
                case FluentTerm term:
                    foreach (var _ in term.Content) yield return _;
                    foreach (var _ in term.Attributes) yield return _;
                    break;
                case FluentMessage message:
                    foreach(var _ in message.Content) yield return _;
                    foreach(var _ in message.Attributes) yield return _;
                    break;
                case FluentAttribute attribute:
                    foreach (var _ in attribute.Content) yield return _;
                    break;
                case FluentPlaceable placeable:
                    yield return placeable.Content;
                    break;
                case FluentSelection selection:
                    yield return selection.Match;
                    foreach(var _ in selection.Variants) yield return _;
                    break;
                case FluentVariant variant:
                    foreach (var _ in variant.Content) yield return _;
                    break;
                case FluentFunctionCall functionCall:
                    foreach (var _ in functionCall.Arguments) yield return _;
                    break;
                case FluentTermReference termCall:
                    foreach (var _ in termCall.Arguments) yield return _;
                    break;
                case FluentCallArgument argument:
                    yield return argument.Content;
                    break;
            }
        }
    }
}
