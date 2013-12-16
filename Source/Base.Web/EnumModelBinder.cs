using System;
using System.Runtime.InteropServices;
using System.Web.Mvc;

namespace Base.Web
{
	public class EnumModelBinder<T> : DefaultModelBinder
		where T : struct
	{
		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
			if (value != null)
			{
				long result = 0;
				foreach (string rawValue in value.RawValue as string[])
				{
					T assignedFlag;
					if (Enum.TryParse(rawValue, out assignedFlag))
					{
						result |= Convert.ToInt64(assignedFlag);
					}
				}

				T enumResult = ParseEnum(result);
				return enumResult;
			}

			return base.BindModel(controllerContext, bindingContext);
		}

		private T ParseEnum(long value)
		{
			var size = Marshal.SizeOf(Enum.GetUnderlyingType(typeof(T)));
			switch (size)
			{
				case 1:
					return (T)(object)Convert.ToByte(value);
				case 2:
					return (T)(object)Convert.ToInt16(value);
				case 4:
					return (T)(object)Convert.ToInt32(value);
				case 8:
					return (T)(object)Convert.ToInt64(value);
				default:
					throw new InvalidOperationException(string.Format("Unknown enum size {0}", size));
			}
		}
	}
}
