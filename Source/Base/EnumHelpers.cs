using System;
using System.Linq;

namespace Base
{
	public static class EnumHelpers
	{
		public static bool ContainsValue<T>(T enumValue)
		{
			return Enum.GetValues(typeof(T)).Cast<T>().Any(x => x.Equals(enumValue));
		}
	}
}
