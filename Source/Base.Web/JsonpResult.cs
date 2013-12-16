using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Base.Web
{
	public class JsonpResult : JsonResult
	{
		public string Callback { get; private set; }

		public JsonpResult(string callback)
		{
			if (string.IsNullOrWhiteSpace(callback))
			{
				throw new ArgumentException("callback", callback);
			}
			Callback = callback;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			ContentEncoding = ContentEncoding ?? Encoding.Default;

			var response = context.HttpContext.Response;
			response.ContentType = ContentType;
			response.ContentEncoding = ContentEncoding;
			response.Charset = ContentEncoding.HeaderName;

			var serializer = new JavaScriptSerializer();
			string serializedJson = serializer.Serialize(Data);
			response.Write(string.Empty);
			response.BinaryWrite(ContentEncoding.GetBytes(string.Format("{0}({1});", Callback, serializedJson)));
		}
	}
}
