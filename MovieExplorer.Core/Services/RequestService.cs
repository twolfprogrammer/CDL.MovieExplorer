using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;

namespace MovieExplorer.Core {
	internal class RequestService {

		HttpClient client;

		public RequestService() {
			client = new HttpClient(new NativeMessageHandler());
		}

		public Task<T> GetObjectAsync<T>(string apiPath, Dictionary<string, string> queryParameters = null) {
			var url = GenerateRequestUrl(apiPath, queryParameters);
			// TODO: add cache
			return GetObjectAsync<T>(url);
		}

		string GenerateRequestUrl(string apiPath, Dictionary<string, string> queryParameters = null) {
			if (string.IsNullOrWhiteSpace(apiPath)) { return null; }
			if (queryParameters == null) {
				queryParameters = new Dictionary<string, string>();
			}
			queryParameters.Add(MovieApi.ApiKeyValueParameter.Key, MovieApi.ApiKeyValueParameter.Value);
			return UrlHelper.AppendQuerystring(UrlHelper.AppendPath(MovieApi.BaseRequestUrl, apiPath), queryParameters);
		}

		async Task<T> GetObjectAsync<T>(string url) {
			T result = default(T);
			if (!string.IsNullOrWhiteSpace(url)) {
				var request = new HttpRequestMessage(HttpMethod.Get, url);
				try {
					var response = await client.SendAsync(request);
					if (response.IsSuccessStatusCode) {
						var json = await response.Content.ReadAsStringAsync();
						result = JsonConvert.DeserializeObject<T>(json);
					}
				}
				catch (JsonSerializationException ex) {
					// TODO: log exception
				}
				catch (HttpRequestException ex) {
					// TODO: log exception
				}
			}
			return result;
		}
	}
}
