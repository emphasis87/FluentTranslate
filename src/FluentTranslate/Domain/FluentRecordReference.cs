using System.Collections;

namespace FluentTranslate.Domain
{
	public abstract class FluentRecordReference : FluentElement, IFluentExpression, IFluentReference
	{
        public string Id { get; set; }
		public string AttributeId { get; set; }

		public abstract string Reference { get; }
    }
}