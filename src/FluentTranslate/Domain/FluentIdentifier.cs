using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    public class FluentIdentifier : FluentElement, IFluentVariantIdentifier
	{
		public string Value { get; set; } = default!;

		public FluentIdentifier()
		{
		}

		public FluentIdentifier(string value) : this()
		{
			Value = value;
		}
    }
}