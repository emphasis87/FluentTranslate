using System;
using System.Globalization;

namespace FluentTranslate.Infrastructure
{
	public interface ICurrentCultureProvider
	{
		CultureInfo Culture { get; }
	}

	public class CurrentCultureProvider : ICurrentCultureProvider
	{
		private readonly Func<CultureInfo> _provider;
		public CultureInfo Culture => _provider?.Invoke() ?? CultureInfo.CurrentUICulture;

		public CurrentCultureProvider(Func<CultureInfo> provider = null)
		{
			_provider = provider;
		}
	}
}
