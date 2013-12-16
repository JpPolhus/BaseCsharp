using System.Web.Mvc;

namespace Base.Web
{
	public class NoCachingAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			filterContext.HttpContext.Response.PreventCaching();
		}
	}
}
