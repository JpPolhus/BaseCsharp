using System.Web.Mvc;

namespace Base.Web.ActionFilterAttributes
{
	public class CrossDomainEnabledAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);

			var request = filterContext.HttpContext.Request;

			bool shouldHandleRequestAsJsonp = request.Url != null && request.Url.Query.Contains("callback");
			if (shouldHandleRequestAsJsonp)
			{
				filterContext.Result = new JsonpResult(request.QueryString["callback"]) 
				{
					JsonRequestBehavior = JsonRequestBehavior.AllowGet, 
					Data = ((JsonResult)filterContext.Result).Data,
					ContentEncoding = ((JsonResult)filterContext.Result).ContentEncoding,
					ContentType = "text/javascript",
				};
			}
		}
	}
}
