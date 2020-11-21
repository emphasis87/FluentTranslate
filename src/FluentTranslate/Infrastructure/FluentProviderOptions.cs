﻿using System;

namespace FluentTranslate.Infrastructure
{
	public class FluentProviderOptions
	{
		/// <summary>
		/// Heuristic for limiting requesting updates from providers.
		/// </summary>
		public TimeSpan? PollingInterval { get; set; }

		/// <summary>
		/// Heuristic for limiting requesting updates from HTTP providers.
		/// </summary>
		public TimeSpan? HttpPollingInterval { get; set; }
	}
}
