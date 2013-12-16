using System.Collections.Generic;
using System.Linq;

namespace Base
{
	public static class ArrayExtensions
	{
		public static T[] ArrayConcat<T>(this T[] array, T item)
		{
			if (array == null)
			{
				return new[] { item };
			}

			return array.Concat(new[] { item }).ToArray();
		}

		public static T[] ArrayConcat<T>(this T[] array, IEnumerable<T> items)
		{
			if (array == null)
			{
				return items.ToArray();
			}

			return array.Concat(items).ToArray();
		}
	}
}
