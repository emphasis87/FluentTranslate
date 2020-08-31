namespace FluentTranslate.Common.Domain
{
	public class FluentComment : IFluentEntry
	{
		public int Level { get; set; }
		public string Value { get; set; }

		public static bool CanAggregate(FluentComment left, FluentComment right)
		{
			return left.Level == right.Level;
		}

		public static FluentComment Aggregate(FluentComment left, FluentComment right)
		{
			return new FluentComment()
			{
				Level = left.Level,
				Value = $"{left.Value}\r\n{right.Value}",
			};
		}
	}
}