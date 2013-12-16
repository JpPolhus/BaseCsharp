using System;

namespace Base
{
	public static class EventHandlerExtensions
	{
		public static void Raise<T>(this EventHandler<T> eventHandler, object sender, T eventArgs)
			where T : EventArgs
		{
			if (eventHandler != null)
			{
				eventHandler(sender, eventArgs);
			}
		}
	}
}
