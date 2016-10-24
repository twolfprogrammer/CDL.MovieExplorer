using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MovieExplorer.Core {
	public class Movies {
		[JsonProperty(PropertyName = "page")]
		public int Page { get; set; }
		[JsonProperty(PropertyName = "results")]
		public List<Movie> Results { get; set; }
		[JsonProperty(PropertyName = "total_results")]
		public int TotalResults { get; set; }
		[JsonProperty(PropertyName = "total_pages")]
		public int TotalPages { get; set; }
	}
}
