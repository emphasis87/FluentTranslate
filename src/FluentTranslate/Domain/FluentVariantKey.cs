using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    [Entity(FluentTranslateEntities.VariantKey)]
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
