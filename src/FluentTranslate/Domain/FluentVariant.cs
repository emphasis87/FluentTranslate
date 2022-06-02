using System.Collections;
using System.Collections.Generic;

using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    public class FluentVariant : FluentContainer, IFluentContainer, IEnumerable<IFluentContent>
	{
        public bool IsDefault { get; set; }
		public IFluentVariantIdentifier Identifier { get; set; }

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

		public override IEnumerator GetEnumerator() => Content.GetEnumerator();
    }
}