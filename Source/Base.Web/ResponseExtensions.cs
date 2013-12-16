using System;
using System.Web;

namespace Base.Web
{
	public static class ResponseExtensions
	{
		private const string P3P = "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"";

		public static void PreventCaching(this HttpResponseBase response)
		{
			response.AddHeader("Pragma", "no-cache");
			response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
			response.AddHeader("Expires", new DateTime(1970, 1, 1).ToString("R"));
		}

		public static void SetP3PHeaders(this HttpResponseBase response)
		{
			response.AddHeader("P3P", P3P);
		}

		public static void SetP3PHeadersAndPreventCaching(this HttpResponseBase response)
		{
			response.PreventCaching();
			response.SetP3PHeaders();
		}
	}
}
