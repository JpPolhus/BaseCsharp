using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Base.Web
{
	public static class HtmlHelperExtensions
	{
		private static readonly Regex _parseIndex = new Regex(@"(.*?)(\.)?\[(\d+)\]$");
		private static readonly Lazy<string> _version = new Lazy<string>(() => Assembly.GetExecutingAssembly().GetName().Version.ToString());

		public static MvcHtmlString CollectionIndex<TModel>(this HtmlHelper<TModel> htmlHelper)
		{
			var result = _parseIndex.Match(htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix);

			string collectionName = result.Groups[1].Value;
			string indexName = result.Groups[3].Value;

			if (!string.IsNullOrEmpty(result.Groups[2].Value))
			{
				// Fix: Bogus MVC generated code like "foobar.[0].baz" to just "foobar[0].baz"
				htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = string.Format("{0}[{1}]", collectionName, indexName);
			}

			return new MvcHtmlString(string.Format("<input type=\"hidden\" name=\"{0}.index\" autocomplete=\"off\" value=\"{1}\" />", collectionName, indexName));
		}
		public static MvcHtmlString CheckBoxFor<TModel, T>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, T>> expression, T assignedFlag)
		{
			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			string name = GetNameFromExpression(expression.Body);

			long modelValue = Convert.ToInt64(GetAccessFunction(expression, htmlHelper.ViewData.Model));
			long flagValue = Convert.ToInt64(assignedFlag);
			bool isChecked = (modelValue & flagValue) == flagValue;

			var htmlAttributes = new Dictionary<string, object>();
			htmlAttributes.Add("value", assignedFlag.ToString());
			return htmlHelper.CheckBox(name, isChecked, htmlAttributes);
		}

		public static MvcHtmlString IncludeVersionedJs(this HtmlHelper helper, string path)
		{
			return GenerateIncludeJs(GetVersionedContentPath(helper, path));
		}

		public static MvcHtmlString IncludeJs(this HtmlHelper helper, string path)
		{
			return GenerateIncludeJs(GetContentPath(helper, path));
		}

		private static MvcHtmlString GenerateIncludeJs(string fullPath)
		{
			var tagBuilder = new TagBuilder("script");
			tagBuilder.Attributes.Add("type", "text/javascript");
			tagBuilder.Attributes.Add("src", fullPath);

			return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
		}

		public static MvcHtmlString IncludeVersionedCss(this HtmlHelper helper, string path, string media = null)
		{
			return GenerateIncludeCss(GetVersionedContentPath(helper, path), media);
		}

		public static MvcHtmlString IncludeCss(this HtmlHelper helper, string path, string media = null)
		{
			return GenerateIncludeCss(GetContentPath(helper, path), media);
		}

		private static MvcHtmlString GenerateIncludeCss(string fullPath, string media)
		{
			var tagBuilder = new TagBuilder("link");
			tagBuilder.Attributes.Add("href", fullPath);
			tagBuilder.Attributes.Add("rel", "stylesheet");
			if (media != null)
			{
				tagBuilder.Attributes.Add("media", media);
			}

			return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));
		}

		private static string GetContentPath(HtmlHelper helper, string path)
		{
			var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

			return urlHelper.Content(path);
		}

		public static string GetVersionedContentPath(this HtmlHelper helper, string path)
		{
			var sourcePath = GetContentPath(helper, path);

			return string.Format(sourcePath.Contains("?") ? "{0}&v={1}" : "{0}?v={1}", sourcePath, _version.Value);
		}

		private static object GetAccessFunction<TModel, T>(Expression<Func<TModel, T>> expression, TModel model)
		{
			try
			{
				return expression.Compile()(model);
			}
			catch (NullReferenceException)
			{
				return null;
			}
		}

		private static string GetNameFromExpression(Expression expression)
		{
			if (expression is MemberExpression)
			{
				var memberExpression = (MemberExpression)expression;
				return string.Format("{0}.{1}", GetNameFromExpression(memberExpression.Expression), memberExpression.Member.Name).TrimStart('.');
			}

			if (expression is ParameterExpression)
			{
				return string.Empty;
			}

			throw new NotSupportedException("Not supported expression type");
		}
	}
}
