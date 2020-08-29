using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FluentVariant : IFluentElement, IFluentContainer
	{
		public bool IsDefault { get; set; }
		public IFluentVariantKey Key { get; set; }
		public IList<IFluentContent> Content { get; set; }

		public FluentVariant()
		{
			Content = new List<IFluentContent>();
		}
	}
}