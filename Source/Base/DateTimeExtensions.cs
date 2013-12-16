using System;
using System.Collections.Generic;

namespace Base
{
	public static class DateTimeExtensions
	{
		public static int ToUnixTime(this DateTime dt)
		{
			var ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
			return (int)ts.TotalSeconds;
		}

		public static DateTime FromUnixTime(int unixTime)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTime).ToLocalTime();
		}

		public static string ToTimeSinceString(this DateTime dateTime)
		{
			TimeSpan ts = DateTime.Now.Subtract(dateTime);
			if (ts.TotalHours < 1)
			{
				var minutsSince = (int)ts.TotalMinutes;
				return minutsSince == 1 ? "Ca 1 minut siden" : string.Format("Ca {0} minutter siden", minutsSince);
			}

			if (ts.TotalDays < 1)
			{
				var hoursSince = (int)ts.TotalHours;
				return hoursSince == 1 ? "Ca 1 time siden" : string.Format("Ca {0} timer siden", hoursSince);
			}

			return (int)ts.TotalDays == 1 ? "I går" : dateTime.ToString("dd. MMM");
		}

		public static DateTime ToPrecision(this DateTime dateTime, Precision precision)
		{
			switch (precision)
			{
				case Precision.Date:
					return dateTime.Date;

				case Precision.Hour:
					return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);

				case Precision.Minute:
					return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);

				case Precision.Second:
					return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);

				default:
					return dateTime;
			}
		}

		public static IEnumerable<DateTime> DatesInInterval(this DateTime start, DateTime? end, int? limit = null)
		{
			if (!end.HasValue)
			{
				yield return start.Date;
			}
			else
			{
				TimeSpan interval = end.Value.Date - start.Date;
				for (int i = 0; i <= interval.TotalDays && IsWithInLimit(limit, i); i++)
				{
					yield return start.AddDays(i).Date;
				}
			}
		}

		private static bool IsWithInLimit(int? limit, int i)
		{
			return !limit.HasValue || i < limit.Value;
		}
	}
}
