using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentVariant : FluentContainer, IFluentContainer
	{
        public bool IsDefault { get; set; }
		public IFluentVariantIdentifier Identifier { get; set; } = default!;

		public FluentVariant()
		{
			
		}

		public FluentVariant(IFluentVariantIdentifier identifier) : this()
		{
			Identifier = identifier;
		}

		public FluentVariant(IFluentVariantIdentifier identifier, bool isDefault) : this(identifier)
		{
			IsDefault = isDefault;
		}
    }
}