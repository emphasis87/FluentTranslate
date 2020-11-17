using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentTranslate
{
	internal class EqualityHelper
	{
		public static int Combine(params int[] hashCodes)
		{
			unchecked
			{
				var hash = 17;
				foreach (var hashCode in hashCodes)
				{
					hash = hash * 31 + hashCode;
				}

				return hash;
			}
		}

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

		public static bool AreEqual(object x, object y)
		{
			if (x is null || y is null) return false;
			if (x is IEnumerable list1 && y is IEnumerable list2)
			{
				var l1 = list1.Cast<object>().ToArray();
				var l2 = list2.Cast<object>().ToArray();
				return l1.Length == l2.Length 
					&& l1.Zip(l2, AreEqual).All(r => r);
			}

			return x.Equals(y);
		}
	}
}
