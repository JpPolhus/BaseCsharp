using System;

namespace Base
{
	public static class NumericHelpers
	{
		public static bool Equals(double a, double b, double tolerance = 0.00001)
		{
			return Math.Abs(a - b) <= Math.Abs(tolerance);
		}
	}
}
