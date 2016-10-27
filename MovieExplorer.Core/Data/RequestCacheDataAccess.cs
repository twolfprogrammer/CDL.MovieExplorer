using System;
using Newtonsoft.Json;

namespace MovieExplorer.Core {
	public class RequestCacheDataAccess {
		// Keep cached item for 30 minutes
		const long ValidityTimePeriod = TimeSpan.TicksPerMinute * 30;

		SQLiteDatabase database;

		static RequestCacheDataAccess instance;
		public static RequestCacheDataAccess Instance { 
			get{
				if (instance == null) {
					throw new Exception(Core.Values.Errors.RequestCacheServiceNotInitialized);
				}
				return instance;
			}
		}

		private RequestCacheDataAccess() { }

		public static void Initialize(ISQLite sqlite) {
			instance = new RequestCacheDataAccess();
			instance.database = new SQLiteDatabase(Values.SQLite.DatabaseName, sqlite);
			instance.database.CreateTable<CachedRequest>();
		}

		void PergeOldRequests() {
			try {
				var ticks = DateTime.Now.Ticks - ValidityTimePeriod;
				database.ExecuteQuery(Values.SQLite.DeleteCachedRequestLessThanDateTimeQuery, new object[] { ticks });
				database.ExecuteQuery(Values.SQLite.VacuumQuery, new object[] { });
			}
			catch (Exception ex) {
				// TODO: log exception
				System.Diagnostics.Debug.WriteLine($"EXCEPTION: {ex.Message}\n{ex.StackTrace}");
			}
		}

		public void Clear() {
			database.DeleteAll<CachedRequest>();
		}

		public void CacheRequest<T>(string url, T jsonResult) {
			if (string.IsNullOrWhiteSpace(url) || jsonResult == null) {
				return;
			}
			var jsonResultString = SerializeObject(jsonResult);
			if (!string.IsNullOrWhiteSpace(jsonResultString)) {
				CacheRequest(url, jsonResultString);
			}
		}

		void CacheRequest(string url, string jsonResultString) {
			if (string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(jsonResultString)) {
				return;
			}
			try {
				var dateTime = DateTime.Now;
				database.SaveItem(new CachedRequest {
					Url = url,
					Json = jsonResultString,
					DateTime = dateTime.Ticks,
				});
				PergeOldRequests();
			}
			catch (Exception ex) {
				// TODO: log exception
				System.Diagnostics.Debug.WriteLine($"EXCEPTION: {ex.Message}\n{ex.StackTrace}");
			}
		}

		public T GetCachedRequest<T>(string url) {
			if (string.IsNullOrWhiteSpace(url)) {
				return default(T);
			}
			var CachedRequest = GetCachedRequest(url);
			if (CachedRequest != null) {
				var jsonResult = DeserializeObject<T>(CachedRequest.Json);
				if (jsonResult == null) {
					try {
						// Cached item is corrupted, remove item
						database.DeleteItem<CachedRequest>(CachedRequest.ID);
					}
					catch (Exception ex) {
						// TODO: log exception
						System.Diagnostics.Debug.WriteLine($"EXCEPTION: {ex.Message}\n{ex.StackTrace}");
					}
				}
				return jsonResult;
			}
			return default(T);
		}

		CachedRequest GetCachedRequest(string url) {
			if (string.IsNullOrWhiteSpace(url)) {
				return null;
			}
			PergeOldRequests();
			try {
				var item = database.FindWithQuery<CachedRequest>(
					Values.SQLite.SelectCachedRequestByUrlQuery, new object[] { url });
				return item;
			}
			catch (Exception ex) {
				// TODO: log exception
				System.Diagnostics.Debug.WriteLine($"EXCEPTION: {ex.Message}\n{ex.StackTrace}");
				return null;
			}
		}

		string SerializeObject<T>(T jsonResult) {
			try {
				var jsonResultString = JsonConvert.SerializeObject(jsonResult);
				return jsonResultString;
			}
			catch (Exception ex) {
				// TODO: log exception
				System.Diagnostics.Debug.WriteLine($"EXCEPTION: {ex.Message}\n{ex.StackTrace}");
				return null;
			}
		}

		T DeserializeObject<T>(string jsonResultString) {
			try {
				var jsonResult = JsonConvert.DeserializeObject<T>(jsonResultString);
				return jsonResult;
			}
			catch (Exception ex) {
				// TODO: log exception
				System.Diagnostics.Debug.WriteLine($"EXCEPTION: {ex.Message}\n{ex.StackTrace}");
				return default(T);
			}
		}
	}
}
