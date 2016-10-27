using System;
namespace MovieExplorer.Core {
	public class CachedRequest : SQLiteBaseItem {
		public string Url { get; set; }
		public string Json { get; set; }
		public long DateTime { get; set; }
		public DateTime GetPrettyDateTime() {
			return new DateTime(DateTime);
		}
	}
}
