using System;
using System.Linq;
using System.Threading;

namespace Base
{
	public static class RetryAction
	{
		public static void Retry<T>(this Action action, int retryCount)
			where T : Exception
		{
			Retry(action, retryCount, typeof(T));
		}

		public static void Retry<T>(this Action action, int retryCount, TimeSpan retryDelay)
			where T : Exception
		{
			Retry(action, retryCount, typeof(T));
		}

		/// <remarks>
		/// If no exception types is given then retry is done on all exceptions.
		/// </remarks>
		public static void Retry(this Action action, int retryCount, params Type[] retryExceptionTypes)
		{
			Retry(action, retryCount, TimeSpan.Zero, retryExceptionTypes);
		}

		/// <remarks>
		/// If no exception types is given then retry is done on all exceptions.
		/// </remarks>
		public static void Retry(this Action action, int retryCount, TimeSpan retryDelay, params Type[] retryExceptionTypes)
		{
			for (int i = 0; i < retryCount; i++)
			{
				try
				{
					action();
					return;
				}
				catch (Exception exception)
				{
					if (i + 1 < retryCount && (retryExceptionTypes.Length == 0 || retryExceptionTypes.Contains(exception.GetType())))
					{
						if (retryDelay != TimeSpan.Zero)
						{
							Thread.Sleep(retryDelay);
						}

						continue;
					}

					throw;
				}
			}
		}
	}
}
