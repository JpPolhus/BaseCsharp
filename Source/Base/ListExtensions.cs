using System;
using System.Collections.Generic;

namespace Base
{
	public static class ListExtensions
	{
		public static void Remove<T>(this IList<T> collection, Func<T, bool> predicate)
		{
			for (int i = 0; i < collection.Count;)
			{
				if (predicate(collection[i]))
				{
					collection.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}
	}
}
