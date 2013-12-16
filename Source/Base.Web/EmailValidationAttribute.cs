using System.ComponentModel.DataAnnotations;

namespace Base.Web
{
	public class EmailAddressValidationAttribute : RegularExpressionAttribute
	{
		public const string EmailRegex = @"^[^@\s]+@\w([\w\.\-])+\w$";

		public EmailAddressValidationAttribute(string errorMessage)
			: base(EmailRegex)
		{
			ErrorMessage = errorMessage;
		}
	}
}
