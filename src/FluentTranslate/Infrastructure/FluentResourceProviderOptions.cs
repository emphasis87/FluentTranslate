using System;

namespace FluentTranslate.Infrastructure
{
	public class FluentResourceProviderOptions
	{
		/// <summary>
		/// Heuristic for limiting requesting updates from providers.
		/// </summary>
		public TimeSpan? PollingInterval { get; set; }

		/// <summary>
		/// Heuristic for limiting requesting updates from file providers.
		/// </summary>
		public TimeSpan? FilePollingInterval { get; set; }

		/// <summary>
		/// Heuristic for limiting requesting updates from HTTP providers.
		/// </summary>
		public TimeSpan? HttpPollingInterval { get; set; }
	}
}
