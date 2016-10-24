using System;
using Newtonsoft.Json;

namespace MovieExplorer.Core {
	public class Configuration {
		[JsonProperty(PropertyName = "images")]
		public ImageOptions Images { get; set; }
		[JsonProperty(PropertyName = "change_keys")]
		public string[] ChangeKeys { get; set; }
	}
}
