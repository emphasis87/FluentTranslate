using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentTranslate.Common
{
	public static class EqualityHelper
	{
		public static int Hash(object item)
		{
			return item switch
			{
				null => 0,
				IEnumerable list => Hash(list),
				{ } other => other.GetHashCode(),
			};
		}

		public static int Hash(IEnumerable items)
        {
			if (items is null)
				return 0;

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

		public static int Hash(params object[] items)
		{
			return Hash(items as IEnumerable);
		}

		public static bool AreEqual<T>(IEnumerable<T> x, IEnumerable<T> y, IEqualityComparer<T> comparer = null)
		{
			if (x is null || y is null) return false;
			if (ReferenceEquals(x, y)) return true;
			return ZipOrDefault(x, y, (e1, e2) => AreEqual(e1, e2, comparer))
				.All(r => r);
		}

		public static bool AreEqual(IEnumerable x, IEnumerable y, IEqualityComparer comparer = null)
		{
			if (x is null || y is null) return false;
			if (ReferenceEquals(x, y)) return true;
			return ZipOrDefault(x.Cast<object>(), y.Cast<object>(), (e1, e2) => AreEqual(e1, e1, comparer))
				.All(r => r);
		}

		public static bool AreEqual<T>(T x, T y, IEqualityComparer<T> comparer = null)
		{
			if (x is null || y is null) return false;
			if (ReferenceEquals(x, y)) return true;
			return comparer?.Equals(x, y) ?? x.Equals(y);
		}

		public static bool AreEqual(object x, object y, IEqualityComparer comparer = null)
		{
			if (x is null || y is null) return false;
			if (ReferenceEquals(x, y)) return true;
			return comparer?.Equals(x, y) ?? x.Equals(y);
		}

		public static IEnumerable<TResult> ZipOrDefault<T1, T2, TResult>(
			IEnumerable<T1> s1!!,
			IEnumerable<T2> s2!!, 
			Func<T1, T2, TResult> resultSelector!!)
        {
            IEnumerator<T1> e1 = null;
			IEnumerator<T2> e2 = null;

            try
            {
				e1 = s1.GetEnumerator();
				e2 = s2.GetEnumerator();

				while (e1 is not null || e2 is not null)
                {
					var v1 = ReadNext(ref e1);
					var v2 = ReadNext(ref e2);

					yield return resultSelector(v1, v2);
                }
            }
            finally
            {
				e1?.Dispose();
				e2?.Dispose();
            }

            static T ReadNext<T>(ref IEnumerator<T> e)
            {
				if (e is null) return default;
				if (e.MoveNext()) return e.Current;
                else
                {
					e.Dispose();
					e = null;
					return default;
                }
            }
		}
	}
}