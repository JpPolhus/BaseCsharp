using System;

namespace Base
{
	public static class Ensure
	{
		public static void IsNotNull(object obj, string parameterName)
		{
			if (obj == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}

		public static void IsNotNullOrEmpty(string value, string parameterName)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentException(parameterName);
			}
		}

		public static void IsTrue(bool condition, string parameterName, string message = null)
		{
			if (!condition)
			{
				throw new ArgumentException(message, parameterName);
			}
		}
		public static void IsNotNullOrWhitespace(string value, string parameterName, string message = null)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName, message);
			}

			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException(message, parameterName);
			}
		}
	}
}
