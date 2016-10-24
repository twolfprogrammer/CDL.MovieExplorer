using System;
using Newtonsoft.Json;

namespace MovieExplorer.Core {
	public class ImageOptions {
		[JsonProperty(PropertyName = "base_url")]
		public string BaseUrl { get; set; }
		[JsonProperty(PropertyName = "secure_base_url")]
		public string SecureBaseUrl { get; set; }
		[JsonProperty(PropertyName = "backdrop_sizes")]
		public string[] BackdropSizes { get; set; }
		[JsonProperty(PropertyName = "logo_sizes")]
		public string[] LogoSizes { get; set; }
		[JsonProperty(PropertyName = "poster_sizes")]
		public string[] PosterSizes { get; set; }
		[JsonProperty(PropertyName = "profile_sizes")]
		public string[] ProfileSizes { get; set; }
		[JsonProperty(PropertyName = "still_sizes")]
		public string[] StillSizes { get; set; }
	}
}
