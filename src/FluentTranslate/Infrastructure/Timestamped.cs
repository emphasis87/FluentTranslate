using System;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate.Infrastructure
{
	public class Timestamped<T>
	{
		public DateTime Timestamp { get; }
		public T Value { get; }

		public Timestamped(DateTime timestamp, T value)
		{
			Timestamp = timestamp;
			Value = value;
		}
	}
}
