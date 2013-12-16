using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Base.Web
{
	public static class DataContractJsonSerializerExtensions
	{
		public static T Deserialize<T>(this DataContractJsonSerializer serializer, string json)
		{
			using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{
				return (T)serializer.ReadObject(memoryStream);
			}
		}

		public static string Serialize<T>(this DataContractJsonSerializer serializer, T instance, int bufferSize = 4096)
		{
			using (var memoryStream = new MemoryStream(bufferSize))
			{
				serializer.WriteObject(memoryStream, instance);
				return Encoding.UTF8.GetString(memoryStream.ToArray());
			}
		}
	}
}
