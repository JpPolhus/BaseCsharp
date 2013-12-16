using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Base.Web.ModelBinders
{
	public class SetModelBinder<T> : IModelBinder
	{
		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var set = new HashSet<T>();

			var rawValues = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).RawValue;
			if (rawValues != null)
			{
				var enumerable = rawValues as IEnumerable;
				if (enumerable != null)
				{
					foreach (var value in enumerable)
					{
						Add(set, value);
					}
				}
				else
				{
					Add(set, rawValues);
				}
			}

			return set;
		}

		private void Add(ISet<T> set, object value)
		{
			if (value is T)
			{
				set.Add((T)value);
			}
		}
	}
}
