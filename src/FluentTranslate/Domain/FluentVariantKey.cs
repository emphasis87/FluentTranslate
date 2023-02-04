using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    public class FluentVariantKey : FluentElement
    {
        public IFluentVariantIdentifier? Identifier { get; set; } = default!;

        public FluentVariantKey()
        {
        }

        public FluentVariantKey(IFluentVariantIdentifier identifier)
        {
            Identifier = identifier;
        }
    }
}
