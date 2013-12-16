using System.Runtime.Caching;

namespace Politiken.Base
{
	public static class MemoryCacheExtensions
	{
		public static T Get<T>(this MemoryCache memoryCache, string key, string regionName = null)
		{
			return (T)memoryCache.Get(key, regionName);
		}
	}
}
