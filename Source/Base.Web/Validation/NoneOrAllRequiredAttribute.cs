using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Base.Web.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class NoneOrAllRequiredAttribute : ValidationAttribute
	{
		private readonly string _group;

		public NoneOrAllRequiredAttribute(string group)
		{
			_group = group;
		}

		public string Group { get { return _group; } }

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var groupProperties = GetPropertiesInGroup(validationContext);

			var values = groupProperties
				.Select(x => x.GetValue(validationContext.ObjectInstance, null))
				.ToList();

			var empty = IsEmpty(value);
			if (values.Any(x => IsEmpty(x) != empty))
			{
				if (empty)
				{
					return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
				}
			}

			return null;
		}

		private bool IsEmpty(object value)
		{
			if (value == null)
			{
				return true;
			}

			var stringValue = value as string;
			if (stringValue != null)
			{
				return string.IsNullOrEmpty(stringValue);
			}

			return false;
		}

		private IEnumerable<PropertyInfo> GetPropertiesInGroup(ValidationContext validationContext)
		{
			var groupProperties = validationContext.ObjectType
				.GetProperties()
				.Select(x => new { property = x, attributes = x.GetCustomAttributes<NoneOrAllRequiredAttribute>() })
				.Where(x => x.attributes.Any(a => a.Group == _group))
				.Select(x => x.property);

			return groupProperties;
		}
	}
}
