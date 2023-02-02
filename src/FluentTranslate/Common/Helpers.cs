namespace FluentTranslate.Common
{
	internal static class Helpers
	{
		public static int Hash(object? item)
		{
			return item switch
			{
				null => 0,
				IEnumerable list => Hash(list),
				{ } other => other.GetHashCode(),
			};
		}

		public static int Hash(IEnumerable? items)
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

		public static int Hash(params object?[] items)
		{
			return Hash(items as IEnumerable);
		}

		public static bool AreEqual<T>(IEnumerable<T>? x, IEnumerable<T>? y, IEqualityComparer<T>? comparer = null)
		{
			if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return ZipOrDefault(x, y, (e1, e2) => AreEqual(e1, e2, comparer))
				.All(r => r);
		}

		public static bool AreEqual(IEnumerable? x, IEnumerable? y, IEqualityComparer? comparer = null)
		{
			if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return ZipOrDefault(x.Cast<object>(), y.Cast<object>(), (e1, e2) => AreEqual(e1, e1, comparer))
				.All(r => r);
		}

		public static bool AreEqual<T>(T? x, T? y, IEqualityComparer<T>? comparer = null)
		{
			if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return comparer?.Equals(x, y) ?? x.Equals(y);
		}

		public static bool AreEqual(object? x, object? y, IEqualityComparer? comparer = null)
		{
			if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return comparer?.Equals(x, y) ?? x.Equals(y);
		}

		public static IEnumerable<TResult> ZipOrDefault<T1, T2, TResult>(
			IEnumerable<T1>? s1,
			IEnumerable<T2>? s2, 
			Func<T1?, T2?, TResult> resultSelector)
        {
			s1 ??= Enumerable.Empty<T1>();
			s2 ??= Enumerable.Empty<T2>();

            var e1 = s1.GetEnumerator();
			var e2 = s2.GetEnumerator();

            try
            {
				bool m1, m2;
				T1? v1;
				T2? v2;
				while(true)
				{
					m1 = e1.MoveNext();
					m2 = e2.MoveNext();

					if (!(m1 || m2))
						break;
					
					v1 = m1 ? e1.Current : default;
					v2 = m2 ? e2.Current : default;

					yield return resultSelector(v1, v2);
				}
            }
            finally
            {
				e1?.Dispose();
				e2?.Dispose();
            }
		}
	}
}