using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public abstract class FluentLocalizedProvider : FluentInvariantProvider
	{
		protected Dictionary<CultureInfo, DateTime> LastPolledByCulture { get; }
		protected Dictionary<CultureInfo, Timestamped<FluentResource>> LastResultByCulture { get; }

		protected FluentLocalizedProvider(IFluentConfiguration configuration)
			: base(configuration)
		{
			LastPolledByCulture = new Dictionary<CultureInfo, DateTime>();
			LastResultByCulture = new Dictionary<CultureInfo, Timestamped<FluentResource>>();
		}

		protected override CultureInfo GetCulture(CultureInfo culture = null)
		{
			return culture ?? CultureInfo.InvariantCulture;
		}

		protected override bool GetLastPolled(out DateTime lastPolled, CultureInfo culture = null)
		{
			culture ??= GetCulture();
			return LastPolledByCulture.TryGetValue(culture, out lastPolled);
		}

		protected override void SetLastPolled(DateTime lastPolled, CultureInfo culture = null)
		{
			culture ??= GetCulture();
			LastPolledByCulture[culture] = lastPolled;
		}

		protected override bool GetLastResult(out Timestamped<FluentResource> lastResult, CultureInfo culture = null)
		{
			culture ??= GetCulture();
			return LastResultByCulture.TryGetValue(culture, out lastResult);
		}

		protected override void SetLastResult(Timestamped<FluentResource> lastResult, CultureInfo culture = null)
		{
			culture ??= GetCulture();
			LastResultByCulture[culture] = lastResult;
		}
	}
}
