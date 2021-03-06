using System;

namespace Base
{
	public interface IDateTimeProvider
	{
		DateTime Now { get; }
		DateTime UtcNow { get; }
		DateTime Today { get; }
	}
}
