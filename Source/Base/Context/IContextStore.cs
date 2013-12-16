namespace Base.Context
{
	public interface IContextStore
	{
		object GetData(string name);

		void SetData(string name, object data);

		void Clear(string name);
	}
}
