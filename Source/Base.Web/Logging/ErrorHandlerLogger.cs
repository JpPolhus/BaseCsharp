using System;
using System.Text;
using System.Web;
using Common.Logging;

namespace Politiken.Base.Web.Logging
{
	public class ErrorHandlerLogger
	{
		private readonly ILog _logger;

		public ErrorHandlerLogger(ILog logger)
		{
			_logger = logger;
		}

		public void HandleError(HttpServerUtility server, HttpRequest request, object sender, EventArgs eventArgs)
		{
			Exception exception = server.GetLastError();

			var httpException = exception as System.Web.HttpException;
			if (httpException != null)
			{
				switch (httpException.GetHttpCode())
				{
					case 404:
						// ignore not found, those can be logged by normal http logging
						return;
				}
			}

			var errorText = FormatErrorMessage(request);

			_logger.Error(errorText, exception);
		}

		private string FormatErrorMessage(HttpRequest request)
		{
			var errorText = new StringBuilder();
			errorText.Append("Unknown HTTP error");

			if (request != null)
			{
				errorText.AppendFormat(", Url{{{0}}}", request.Url.AbsoluteUri);
			}

			if (request != null && request.UrlReferrer != null)
			{
				errorText.AppendFormat(", UrlReferrer{{{0}}}", request.UrlReferrer.AbsoluteUri);
			}

			return errorText.ToString();
		}
	}
}
