using FluentTranslate.Domain;

namespace FluentTranslate.Domain.Common
{
	public abstract class FluentRecordReference : FluentElement, IFluentExpression, IFluentTargetReference
	{
        public string TargetId { get; set; }
		public string TargetAttributeId { get; set; }
		public abstract string TargetReference { get; }

		public static FluentRecordReference Create(string reference)
		{
			var result = reference.StartsWith("-")
				? (FluentRecordReference) new FluentTermReference()
				: new FluentMessageReference();

			var dotIndex = reference.IndexOf('.');
			if (dotIndex != -1)
			{
				var attribute = reference.Substring(dotIndex + 1);
				result.TargetAttributeId = attribute;
				reference = reference.Substring(0, dotIndex);
			}

			result.TargetId = reference;
			return result;
		}
    }
}