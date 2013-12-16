using System;

namespace Base.Web
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class ControllerRouteAttribute : Attribute
	{
		public string ControllerRoute { get; set; }

		public ControllerRouteAttribute(string controllerRoute)
		{
			ControllerRoute = controllerRoute;
		}
	}
}
