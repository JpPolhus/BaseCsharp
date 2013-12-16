using System;

namespace Base
{
	public static class Guard
	{
		public static void IsNotEmpty(this string value, string parameterName)
		{
			if (value == string.Empty) { throw new ArgumentException("Parameter cannot be empty", parameterName); }
		}

		public static void IsNotNullOrEmpty(this string value, string parameterName)
		{
			Ensure.IsNotNull(value, parameterName);
			value.IsNotEmpty(value);
		}
	}
}
