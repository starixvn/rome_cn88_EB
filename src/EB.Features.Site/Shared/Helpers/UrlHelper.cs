using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace EB.Features.Site.Shared.Helpers
{
    public static class UrlHelper
    {
        [ThreadStatic]
        private static Func<string, IDictionary<string, object>, string> replaceOrAddQueryStringParamsFunc;


        public static Func<string, IDictionary<string, object>, string> ReplaceOrAddQueryStringParamsFunc
        {
            get
            {
                return replaceOrAddQueryStringParamsFunc ?? ReplaceOrAddQueryStringParams;
            }

            internal set
            {
                replaceOrAddQueryStringParamsFunc = value;
            }
        }

        internal static string ReplaceOrAddQueryStringParams(string link, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return link;
            }

            if (!link.Contains("?"))
            {
                return AddQueryStringParams(link, routeValues);
            }

            foreach (var routeValue in routeValues)
            {
                link = ReplaceOrAddQueryStringParam(link, routeValue.Key, routeValue.Value);
            }

            return link;
        }

        internal static string AddQueryStringParams(string link, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return link;
            }

            var queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"{link}?{queryString}";
        }

        internal static string ReplaceOrAddQueryStringParam(string link, string paramName, object paramValue)
        {
            if (string.IsNullOrWhiteSpace(paramName) || paramValue == null)
            {
                return link;
            }

            var regexParamName = @"([?&]" + paramName + ")=[^?&]+";
            if (Regex.IsMatch(link, regexParamName, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(link, regexParamName, "$1=" + paramValue, RegexOptions.IgnoreCase);
            }

            return link + $"&{paramName}={paramValue}";
        }
    }
}