using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentVariant : FluentContainer, IFluentContainer
	{
        public bool IsDefault { get; set; }
		public FluentVariantKey Key { get; set; } = default!;

		public FluentVariant()
		{
			
		}

		public FluentVariant(FluentVariantKey key) : this()
		{
			Key = key;
		}

        public FluentVariant(IFluentVariantIdentifier identifier) : this()
        {
            Key = new FluentVariantKey(identifier);
        }

        public FluentVariant(IFluentVariantIdentifier identifier, bool isDefault) : this(identifier)
		{
			IsDefault = isDefault;
		}
    }
}