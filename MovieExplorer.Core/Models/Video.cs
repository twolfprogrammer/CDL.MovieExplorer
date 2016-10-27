using System;
using Newtonsoft.Json;

namespace MovieExplorer.Core {
	public class Video {
		public string Id { get; set; }
		[JsonProperty(PropertyName = "iso_639_1")]
		public string Iso6391 { get; set; }
		[JsonProperty(PropertyName = "iso_3166_1")]
		public string Iso31661 { get; set; }
		public string Key { get; set; }
		public string Name { get; set; }
		public string Site { get; set; }
		public int Size { get; set; }
		public string Type { get; set; }
	}
}
