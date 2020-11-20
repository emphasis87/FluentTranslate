using System;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate.Infrastructure
{
	public class FluentProviderOptions
	{
		/// <summary>
		/// Heuristic for limiting requesting updates from providers.
		/// </summary>
		public TimeSpan? PollingInterval { get; set; }
	}
}
