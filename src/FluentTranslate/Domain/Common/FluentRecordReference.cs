namespace FluentTranslate.Domain.Common
{
	public abstract class FluentRecordReference : FluentElement, IFluentExpression, IFluentReference
	{
		public string Id { get; set; } = default!;
		public string? AttributeId { get; set; } = default!;

		public abstract string Target { get; }

		public static FluentRecordReference Create(string target)
		{
			var result = target.StartsWith("-")
				? (FluentRecordReference) new FluentTermReference()
				: new FluentMessageReference();

			var dotIndex = target.IndexOf('.');
			if (dotIndex != -1)
			{
				var attribute = target.Substring(dotIndex + 1);
				result.AttributeId = attribute;
				target = target.Substring(0, dotIndex);
			}

			result.Id = target;
			return result;
		}
    }
}