using System.Text;
using System.Web.Mvc;

namespace Base.Web.ActionFilterAttributes
{
	public class ChangableEncodingAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			base.OnActionExecuted(filterContext);

			var request = filterContext.HttpContext.Request;

			bool shouldChangeEncoding = request.Url != null && (request.Url.Query.Contains("encoding") || request.Headers.Get("encoding") != null);
			if (shouldChangeEncoding)
			{
				string encoding = request.Url.Query.Contains("encoding") ? request.QueryString["encoding"] : request.Headers.Get("encoding");
				((JsonResult)filterContext.Result).ContentEncoding = Encoding.GetEncoding(encoding);
			}
		}
	}
}
