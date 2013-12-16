using System.Text.RegularExpressions;

namespace Base.Web
{
	// Stolen from iByen
	public static class SearchEngineOptimizationsHelper
	{
		public static string ToUrlSafeString(this string url)
		{
			// make the url lowercase
			string encodedUrl = (url ?? "").ToLower();

			// replace danish characters
			encodedUrl = Regex.Replace(encodedUrl, "æ", "ae");
			encodedUrl = Regex.Replace(encodedUrl, "ø", "oe");
			encodedUrl = Regex.Replace(encodedUrl, "å", "aa");

			// replace accented characters
			encodedUrl = Regex.Replace(encodedUrl, "[ã|à|â|ä|á]", "a");
			encodedUrl = Regex.Replace(encodedUrl, "[é|è|ê|ë]", "e");
			encodedUrl = Regex.Replace(encodedUrl, "[í|ì|î|ï]", "i");
			encodedUrl = Regex.Replace(encodedUrl, "ñ", "n");
			encodedUrl = Regex.Replace(encodedUrl, "[õ|ò|ó|ô|ö]", "o");
			encodedUrl = Regex.Replace(encodedUrl, "[ù|ú|û|ü|µ]", "u");

			// replace & with og
			encodedUrl = Regex.Replace(encodedUrl, @"\&+", "og");

			// remove characters
			encodedUrl = encodedUrl.Replace("'", "");

			// remove invalid characters
			encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

			// remove duplicates
			encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

			// trim leading & trailing characters
			encodedUrl = encodedUrl.Trim('-');

			return encodedUrl;
		}
	}
}
