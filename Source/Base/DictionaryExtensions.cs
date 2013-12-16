using System;
using System.Collections.Generic;

namespace Base
{
	public static class DictionaryExtensions
	{
		public static void AddRange<T, TKey>(this IDictionary<TKey, IList<T>> dictionary, Func<T, TKey> keySelector, IEnumerable<T> enumerable)
		{
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
		}

		public static void AddRange<T, TKey>(this IDictionary<TKey, IList<T>> dictionary, Func<T, TKey> keySelector, params T[] items)
		{
			dictionary.AddRange(keySelector, (IEnumerable<T>)items);
		}

		public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> createMethod)
		{
			TValue value;
			if (!dictionary.TryGetValue(key, out value))
			{
				dictionary[key] = value = createMethod(key);
			}

			return value;
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
		{
			TValue value;
			if (dictionary.TryGetValue(key, out value))
			{
				return value;
			}

			return defaultValue;
		}
	}
}
