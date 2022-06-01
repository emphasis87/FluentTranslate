using System.Collections;

namespace FluentTranslate.Domain
{
	public abstract class FluentRecordReference : FluentElement, IFluentExpression, IFluentReference
	{
        public string Id { get; set; }
		public string AttributeId { get; set; }

		public abstract string Reference { get; }

		public static FluentRecordReference Create(string reference)
		{
			var result = reference.StartsWith("-")
				? (FluentRecordReference) new FluentTermReference()
				: new FluentMessageReference();

			var dotIndex = reference.IndexOf('.');
			if (dotIndex != -1)
			{
				var attribute = reference.Substring(dotIndex + 1);
				result.AttributeId = attribute;
				reference = reference.Substring(0, dotIndex);
			}

			result.Id = reference;
			return result;
		}
    }
}