using System;
using System.Collections.Generic;

namespace Base
{
	public static class TypeExtensions
	{
		public static IEnumerable<Type> GetTypeHierarchy(this Type type)
		{
			while (type != null)
			{
				yield return type;
				type = type.BaseType;
			}
		}
	}
}
