using System;
using System.Collections.Generic;
using System.Linq;

namespace Base
{
	public static class EnumExtensions
	{
		public static IEnumerable<T> GetValues<T>()
			where T : struct
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		public static IEnumerable<Enum> GetFlags(this Enum baseEnum, bool includeConcatFlags = false)
		{
			ulong enumValue = Convert.ToUInt64(baseEnum);
			foreach (var x in Enum.GetValues(baseEnum.GetType()))
			{
				ulong value = Convert.ToUInt64(x);
				if (value == 0 || (!includeConcatFlags && NumberOfSetBits(value) > 1))
				{
					continue;
				}

				if ((enumValue & value) == value)
				{
					yield return (Enum)x;
				}
			}
		}

		public static IEnumerable<T> GetFlags<T>(this Enum baseEnum, bool includeConcatFlags = false)
		{
			ulong enumValue = Convert.ToUInt64(baseEnum);
			foreach (var x in Enum.GetValues(typeof(T)))
			{
				ulong value = Convert.ToUInt64(x);
				if (value == 0 || (!includeConcatFlags && NumberOfSetBits(value) > 1))
				{
					continue;
				}

				if ((enumValue & value) == value)
				{
					yield return (T)x;
				}
			}
		}

		// Hamming Weight algorithm for bit counting
		// See: http://stackoverflow.com/questions/2709430/count-number-of-bits-in-a-64-bit-long-big-integer
		private static int NumberOfSetBits(ulong i)
		{
			i = i - ((i >> 1) & 0x5555555555555555UL);
			i = (i & 0x3333333333333333UL) + ((i >> 2) & 0x3333333333333333UL);
			return (int)(unchecked(((i + (i >> 4)) & 0xF0F0F0F0F0F0F0FUL) * 0x101010101010101UL) >> 56);
		}
	}
}
