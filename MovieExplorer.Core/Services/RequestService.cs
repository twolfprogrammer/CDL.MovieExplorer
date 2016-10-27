using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;

namespace MovieExplorer.Core {
	internal class RequestService {

		HttpClient client;

		static RequestService instance;
		public static RequestService Instance {
			get {
				return instance ?? (instance = new RequestService());
			}
		}

		private RequestService() {
			client = new HttpClient(new NativeMessageHandler());
		}

		public async Task<T> GetObjectAsync<T>(string apiPath, Dictionary<string, string> queryParameters = null) {
			var url = GenerateRequestUrl(apiPath, queryParameters);
			var cachedRequest = RequestCacheDataAccess.Instance.GetCachedRequest<T>(url);
			if (cachedRequest != null) {
				System.Diagnostics.Debug.WriteLine($"Cached item found {typeof(T).ToString()} for: {url}");
				return cachedRequest;
			}
			return await GetObjectAsync<T>(url);
		}

		string GenerateRequestUrl(string apiPath, Dictionary<string, string> queryParameters = null) {
			if (string.IsNullOrWhiteSpace(apiPath)) { return null; }
			if (queryParameters == null) {
				queryParameters = new Dictionary<string, string>();
			}
			queryParameters.Add(Values.MovieApi.ApiKeyValueParameter.Key, Values.MovieApi.ApiKeyValueParameter.Value);
			return UrlHelper.AppendQuerystring(UrlHelper.AppendPath(Values.MovieApi.BaseRequestUrl, apiPath), queryParameters);
		}

		async Task<T> GetObjectAsync<T>(string url) {
			System.Diagnostics.Debug.WriteLine($"Fetching: {url}");
			T result = default(T);
			if (!string.IsNullOrWhiteSpace(url)) {
				var request = new HttpRequestMessage(HttpMethod.Get, url);
				try {
					var response = await client.SendAsync(request);
					if (response.IsSuccessStatusCode) {
						var json = await response.Content.ReadAsStringAsync();
						result = JsonConvert.DeserializeObject<T>(json);
						RequestCacheDataAccess.Instance.CacheRequest<T>(url, result);
					}
				}
				catch (Exception ex) {
					// TODO: log exception
					System.Diagnostics.Debug.WriteLine($"EXCEPTION: {ex.Message}\n{ex.StackTrace}");
				}
			}
			return result;
		}
	}
}
