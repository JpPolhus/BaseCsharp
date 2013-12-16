using System;

namespace Base
{
	public class DateTimeProvider : IDateTimeProvider
	{
		public DateTime Now
		{
			get
			{
				return DateTime.Now;
			}
		}

		public DateTime UtcNow
		{
			get
			{
				return DateTime.UtcNow;
			}
		}

		public DateTime Today
		{
			get
			{
				return DateTime.Today;
			}
		}
	}
}
