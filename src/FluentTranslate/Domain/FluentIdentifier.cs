using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    public class FluentIdentifier : FluentElement, IFluentVariantIdentifier
	{
        public string Value { get; set; }

		public FluentIdentifier()
		{
		}

		public FluentIdentifier(string value)
		{
			Value = value;
		}
    }
}