using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Base
{
	public static class EnumerableExtensions
	{
		public static IDictionary<TKey, IList<T>> ToDictionaryList<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector)
		{
			var dictionary = new Dictionary<TKey, IList<T>>();
			dictionary.AddRange(keySelector, enumerable);
			return dictionary;
		}

		public static void Do<T>(this IEnumerable<T> collection, Action<T> action)
		{
			Ensure.IsNotNull(collection, "collection");
			Ensure.IsNotNull(action, "action");

			foreach (T item in collection)
			{
				action(item);
			}
		}

		public static bool SequenceEqual(IEnumerable a, IEnumerable b)
		{
			if (a == null && b == null)
			{
				return true;
			}
			if (a == null)
			{
				return !b.GetEnumerator().MoveNext();
			}
			if (b == null)
			{
				return !a.GetEnumerator().MoveNext();
			}

			var aEnumerator = a.GetEnumerator();
			var bEnumerator = b.GetEnumerator();

			while (true)
			{
				var aMove = aEnumerator.MoveNext();
				var bMove = bEnumerator.MoveNext();
				if (aMove != bMove)
				{
					return false;
				}

				if (aMove == false)
				{
					return true;
				}

				if (!Equals(aEnumerator.Current, bEnumerator.Current))
				{
					return false;
				}
			}
		}

		public static IDictionary<TKey, IList<T>> ToDictionaryList<T, TKey>(this IEnumerable<T> enumerable, Func<T, TKey> keySelector, IEqualityComparer<TKey> equalityComparer = null)
		{
			var dictionary = equalityComparer != null ? new Dictionary<TKey, IList<T>>(equalityComparer) : new Dictionary<TKey, IList<T>>();
			foreach (var item in enumerable)
			{
				var key = keySelector(item);

				IList<T> list;
				if (!dictionary.TryGetValue(key, out list))
				{
					dictionary[key] = list = new List<T>();
				}
				list.Add(item);
			}

			return dictionary;
		}

		public static IEnumerable<TValue> SelectIn<TKey, TValue>(this IEnumerable<TKey> enumerable, IDictionary<TKey, TValue> lookup)
		{
			foreach (var key in enumerable)
			{
				TValue value;
				if (lookup.TryGetValue(key, out value))
				{
					yield return value;
				}
			}
		}

		public static bool HasExactly<T>(this IEnumerable<T> enumerable, int count)
		{
			if (enumerable.Count() == count)
			{
				return true;
			}
			return false;
		}

		public static string NaturalJoin(this IEnumerable<string> enumerable, string seperator, string endSeperator)
		{
			var values = enumerable as string[] ?? enumerable.ToArray();
			if (values.Count() > 2)
			{
				return string.Join(seperator, values.Take(values.Count() - 1)) + endSeperator + values.Last();
			}
			return string.Join(endSeperator, values);
		}


		public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> enumerable, int count)
		{
			return enumerable.Reverse().Skip(count).Reverse();
		}

		public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T value)
		{
			return enumerable.Concat(new[] { value });
		}

		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
		{
			return new HashSet<T>(enumerable);
		}

		// Implementation of Fisher-Yates 
		// See: http://stackoverflow.com/questions/1287567/is-using-random-and-orderby-a-good-shuffle-algorithm/1287572#1287572
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
		{
			T[] elements = source.ToArray();
			for (int i = elements.Length - 1; i >= 0; i--)
			{
				int swapIndex = rng.Next(i + 1);
				yield return elements[swapIndex];
				elements[swapIndex] = elements[i];
			}
		}

	}
}
