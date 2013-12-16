using System;
using System.Text;

namespace Base.Formatting
{
	public static class StringExtensions
	{
		/// <summary>
		/// Json compatiable quoting of a string, see RFC-4627
		/// </summary>
		public static string Quote(this string value)
		{
			var result = new StringBuilder(value.Length * 2);
			result.Append('"');
			foreach (char c in value)
			{
				if (c == '"')
				{
					result.Append('\\');
					result.Append(c);
				}
				else if (c == '\\')
				{
					result.Append('\\');
					result.Append(c);
				}
				else if (c == '\t')
				{
					result.Append(@"\t");
				}
				else if (c == '\r')
				{
					result.Append(@"\r");
				}
				else if (c == '\n')
				{
					result.Append(@"\n");
				}
				else if (c <= 0x001F)
				{
					result.AppendFormat("\\u{0:X4}", Convert.ToUInt16(c));
				}
				else
				{
					result.Append(c);
				}
			}
			result.Append('"');

			return result.ToString();
		}
	}
}
