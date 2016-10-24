using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace MovieExplorer.Core {
	public static class UrlHelper {
		public static string AppendPath(string url, string path) {
			if (string.IsNullOrWhiteSpace(url)) { return null; }
			if (string.IsNullOrWhiteSpace(path)) { return url; }
			url = url.RemoveSpaces().TrimSlashes();
			path = path.RemoveSpaces().TrimSlashes();
			return $"{url}/{path}";
		}

		public static string AppendQuerystring(string url, Dictionary<string, string> queryParameters) {
			if (string.IsNullOrWhiteSpace(url)) { return null; }
			if (queryParameters == null || !queryParameters.Any
			    (p => !string.IsNullOrWhiteSpace(p.Key) && !string.IsNullOrWhiteSpace(p.Value))) { return url; }
			var uriBuilder = new UriBuilder(url);
			var queryString = uriBuilder.Query;
			foreach (var pair in queryParameters.Where(kvp => kvp.Value != null)) {
				var key = WebUtility.UrlEncode(pair.Key);
				var value = WebUtility.UrlEncode(pair.Value);
				queryString = string.IsNullOrWhiteSpace(queryString) ?
									$"{key}={value}" : $"{queryString}&{key}={value}";
			}
			uriBuilder.Query = queryString;
			return uriBuilder.Uri.AbsoluteUri;
		}

		static string RemoveSpaces(this string value) {
			return Regex.Replace(value, @"\s+", "");
		}

		static string TrimSlashes(this string value) {
			var charsToTrim = new char[] { '/' };
			return value.Trim(charsToTrim);
		}
	}
}
