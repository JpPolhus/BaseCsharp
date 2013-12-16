using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace Base.Web
{
	public static class HttpCookieCollectionExtensions
	{
		private static readonly ThreadLocal<JavaScriptSerializer> _serializer = new ThreadLocal<JavaScriptSerializer>(() => new JavaScriptSerializer());

		public static void Delete(this HttpCookieCollection cookieCollection, string name, string domain = null, string path = "/")
		{
			var cookie = new HttpCookie(name);
			cookie.Expires = new DateTime(1970, 1, 1);
			cookie.Value = string.Empty;

			if (domain != null)
			{
				cookie.Domain = domain;
			}

			if (path != null)
			{
				cookie.Path = path;
			}

			cookieCollection.Add(cookie);
		}

		public static IEnumerable<HttpCookie> GetCookieEnumerator(this HttpCookieCollection cookieCollection)
		{
			for (int i = 0; i < cookieCollection.Count; i++)
			{
				yield return cookieCollection[i];
			}
		}

		public static void AddDataCookie<T>(this HttpCookieCollection cookieCollection, string name, T data, string domain = null, string path = "/")
			where T : class
		{
			var value = new DataPayload(_serializer.Value.Serialize(data));
			var cookie = new HttpCookie(name, _serializer.Value.Serialize(value))
			{
				Domain = domain,
				Path = path,
			};

			cookieCollection.Add(cookie);
		}

		public static T GetDataCookie<T>(this HttpCookieCollection cookieCollection, string name)
			where T : class
		{
			var cookie = cookieCollection[name];
			if (cookie == null)
			{
				return null;
			}

			try
			{
				var payload = _serializer.Value.Deserialize<DataPayload>(cookie.Value);
				if (payload == null || !payload.IsValid())
				{
					return null;
				}

				return _serializer.Value.Deserialize<T>(payload.Data);
			}
			catch (ArgumentException)
			{
				// invalid json
				return null;
			}
		}

		[Serializable]
		internal class DataPayload
		{
			private static readonly Lazy<string> _payloadKey = new Lazy<string>(() => ConfigurationManager.AppSettings["PayloadKey"]);
			private string _encodedData;

			public DataPayload()
			{
			}

			public DataPayload(string data)
			{
				Data = data;
				Crc = CalculateChecksum();
			}

			public string EncodedData
			{
				get
				{
					return _encodedData;
				}
				set
				{
					_encodedData = value;
				}
			}

			[ScriptIgnore]
			public string Data
			{
				get
				{
					if (_encodedData == null)
					{
						return null;
					}

					var data = Convert.FromBase64String(_encodedData);
					return Encoding.UTF8.GetString(data);
				}
				set
				{
					if (value == null)
					{
						_encodedData = null;
					}
					else
					{
						var data = Encoding.UTF8.GetBytes(value);
						_encodedData = Convert.ToBase64String(data);
					}
				}
			}

			public string Crc { get; set; }

			public bool IsValid()
			{
				return Crc != null && EncodedData != null && Crc == CalculateChecksum();
			}

			private string CalculateChecksum()
			{
				var rawDataKey = string.Format(_payloadKey.Value, EncodedData);
				var data = Encoding.UTF8.GetBytes(rawDataKey);
				return Convert.ToBase64String(MD5.Create().ComputeHash(data));
			}
		}
	}
}
