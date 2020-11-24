using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentTranslate.WebHost.Infrastructure
{
	internal class EqualityHelper
	{
		public static int Hash(object item)
		{
			return item switch
			{
				null => 0,
				IEnumerable list => Combine(list.Cast<object>().ToArray()),
				{ } other => other.GetHashCode(),
			};
		}

		public static int Combine(params object[] items)
		{
			unchecked
			{
				var hash = 17;
				foreach (var item in items)
				{
					hash = hash * 31 + Hash(item);
				}

				return hash;
			}
		}

		public static bool AreEqual<T>(IEnumerable<T> x, IEnumerable<T> y, IEqualityComparer<T> comparer)
		{
			if (x is null || y is null) return false;
			if (ReferenceEquals(x, y)) return true;
			var l1 = x.ToArray();
			var l2 = y.ToArray();
			return l1.Length == l2.Length
				&& l1.Zip(l2, (a, b) => AreEqual(a, b, comparer)).All(r => r);
		}

		public static bool AreEqual(IEnumerable x, IEnumerable y)
		{
			if (x is null || y is null) return false;
			if (ReferenceEquals(x, y)) return true;
			var l1 = x.Cast<object>().ToArray();
			var l2 = y.Cast<object>().ToArray();
			return l1.Length == l2.Length
				&& l1.Zip(l2, AreEqual).All(r => r);
		}

		public static bool AreEqual<T>(T x, T y, IEqualityComparer<T> comparer)
		{
			if (x is null || y is null) return false;
			if (ReferenceEquals(x, y)) return true;
			return comparer?.Equals(x, y) ?? x.Equals(y);
		}

		public static bool AreEqual(object x, object y)
		{
			if (x is null || y is null) return false;
			if (ReferenceEquals(x, y)) return true;
			return x.Equals(y);
		}
	}
}