namespace FluentTranslate.Common.Domain
{
	public class FluentText : IFluentContent
	{
		public string Value { get; set; }

		public static FluentText Aggregate(FluentText left, FluentText right)
		{
			return new FluentText()
			{
				Value = $"{left.Value}{right.Value}"
			};
		}
	}
}