using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public class FluentVariant : FluentElement, IFluentContainer, IEnumerable<IFluentContent>
	{
        public override string Type => FluentElementTypes.Variant;
        public bool IsDefault { get; set; }
		public IFluentVariantKey Key { get; set; }
		public List<IFluentContent> Content { get; internal set; }

		public FluentVariant()
		{
			Content = new List<IFluentContent>();
		}

		public FluentVariant(IFluentVariantKey key) : this()
		{
			Key = key;
		}

		public FluentVariant(IFluentVariantKey key, bool isDefault) : this()
		{
			Key = key;
			IsDefault = isDefault;
		}

		public void Add(IFluentContent content)
		{
			Content.Add(content);
		}

		public IEnumerator<IFluentContent> GetEnumerator() => Content.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}