using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Base
{
	public static class NameValueCollectionExtensions
	{
		public static string Acquire(this NameValueCollection appSettings, string key)
		{
			var value = appSettings[key];

			if (string.IsNullOrEmpty(value)) { throw new ConfigurationErrorsException(string.Format("The app setting with key \"{0}\" is null or empty. Please fix this.", key)); }

			return value;
		}

		public static T Acquire<T>(this NameValueCollection appSettings, string key)
		{
			try
			{
				var value = Acquire(appSettings, key);

				return (T)System.ComponentModel.TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(value);
			}
			catch (FormatException formatException)
			{
				throw new ConfigurationErrorsException(string.Format("The key '{0}' is expected to be of type {1}. Please fix configuration.", key, typeof(T).Name), formatException);
			}
		}
	}
}
