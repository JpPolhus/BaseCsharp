using System;
using System.Net;
using System.Runtime.Serialization;

namespace Base.Web
{
	public class HttpException : System.Web.HttpException
	{
		public HttpException()
		{
		}

		public HttpException(string message)
			: base(message)
		{
		}

		public HttpException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public HttpException(HttpStatusCode statusCode, string message)
			: base((int)statusCode, message)
		{
		}

		public HttpException(HttpStatusCode statusCode, string message, Exception innerException)
			: base((int)statusCode, message, innerException)
		{
		}

		protected HttpException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
