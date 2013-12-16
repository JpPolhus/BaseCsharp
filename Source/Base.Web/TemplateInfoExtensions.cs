using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Base.Web
{
	public static class TemplateInfoExtensions
	{
		private readonly static Regex _fixCollectionPrefix = new Regex(@".(\[\d+\])$", RegexOptions.Compiled);

		public static void FixCollectionPrefix(this TemplateInfo templateInfo)
		{
			string prefix = templateInfo.HtmlFieldPrefix;
			if (!string.IsNullOrEmpty(prefix))
			{
				templateInfo.HtmlFieldPrefix = _fixCollectionPrefix.Replace(prefix, "$1");
			}
		}
	}
}
