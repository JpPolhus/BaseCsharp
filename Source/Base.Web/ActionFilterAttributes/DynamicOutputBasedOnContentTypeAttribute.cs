using System.Web.Mvc;

namespace Base.Web.ActionFilterAttributes
{
	public class DynamicOutputBasedOnContentTypeAttribute : ActionFilterAttribute
	{
		private const string AcceptedTypeHeader = "Accept";
		private const string ContentFormatParameter = "resultFormat";

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);

			var resultContentType = string.Empty;

			var request = filterContext.HttpContext.Request;
			if (request[ContentFormatParameter] != null)
			{
				resultContentType = request[ContentFormatParameter];
			}
			else if (request.Headers[AcceptedTypeHeader] != null)
			{
				resultContentType = filterContext.HttpContext.Request.Headers[AcceptedTypeHeader];
			}

			filterContext.Result = SerilizeHttpResult(filterContext, resultContentType);
		}

		private ActionResult SerilizeHttpResult(ActionExecutedContext filterContext, string resultContentType)
		{
			if (resultContentType.ToLower().Contains("json"))
			{
				var jsonResult = new JsonResult
				{
					JsonRequestBehavior = JsonRequestBehavior.AllowGet
				};

				if (filterContext.Controller.ViewData.Model != null)
				{
					jsonResult.Data = filterContext.Controller.ViewData.Model;
				}

				return jsonResult;
			}

			return filterContext.Result;
		}
	}
}
