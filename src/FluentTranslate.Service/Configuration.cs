using System;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate.Service
{
	public class Configuration
	{
		/// <summary>
		/// Settling period for which to wait until changes in monitored files are committed.
		/// </summary>
		public TimeSpan MonitoringDebounceInterval { get; set; }

		public Configuration()
		{
			MonitoringDebounceInterval = TimeSpan.FromSeconds(60);
		}
	}
}
