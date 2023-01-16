namespace FluentTranslate.Infrastructure
{
	public interface ICurrentCultureProvider
	{
		CultureInfo Culture { get; }
	}

	public class CurrentCultureProvider : ICurrentCultureProvider
	{
		public static ICurrentCultureProvider Default { get; } =
			new CurrentCultureProvider(() => CultureInfo.CurrentUICulture);

		private readonly Func<CultureInfo> _provider;
		public CultureInfo Culture => _provider?.Invoke() ?? CultureInfo.CurrentUICulture;

		public CurrentCultureProvider(Func<CultureInfo> provider = null)
		{
			_provider = provider;
		}
	}
}
