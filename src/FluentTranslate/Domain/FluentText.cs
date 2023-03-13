using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    [Entity(FluentTranslateEntities.Text)]
    public class FluentText : FluentElement, IFluentContent
    {
		public string Value { get; set; } = default!;

		public FluentText()
		{
		}

		public FluentText(string value) : this()
		{
			Value = value;
		}
	}
}