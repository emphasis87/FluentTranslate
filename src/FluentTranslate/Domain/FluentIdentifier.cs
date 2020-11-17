using System.Collections;

namespace FluentTranslate.Domain
{
	public class FluentIdentifier : FluentElement, IFluentVariantKey
    {
        public override string Type => "id";

        public string Id { get; set; }

		public FluentIdentifier()
		{
		}

		public FluentIdentifier(string id)
		{
			Id = id;
		}
    }
}