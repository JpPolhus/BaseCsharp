using System.Web;
using Base.Context;

namespace Base.Web.Context
{
	public class HttpContextStore : IContextStore
	{
		public object GetData(string name)
		{
			if (HttpContext.Current.Items.Contains(name))
			{
				return HttpContext.Current.Items[name];
			}

			return null;
		}

		public void SetData(string name, object data)
		{
			HttpContext.Current.Items[name] = data;
		}

		public void Clear(string name)
		{
			HttpContext.Current.Items.Remove(name);
		}
	}
}
