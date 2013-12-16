using System.Runtime.Remoting.Messaging;

namespace Base.Context
{
	public class CallContextStore : IContextStore
	{
		public object GetData(string name)
		{
			return CallContext.LogicalGetData(name);
		}

		public void SetData(string name, object data)
		{
			CallContext.LogicalSetData(name, data);
		}

		public void Clear(string name)
		{
			CallContext.FreeNamedDataSlot(name);
		}
	}
}
