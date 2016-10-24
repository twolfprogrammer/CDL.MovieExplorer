using System;
using Newtonsoft.Json;

namespace MovieExplorer.Core {
	public class Movie {
		[JsonProperty(PropertyName = "poster_path")]
		public string PosterImagePath { get; set; }
		[JsonProperty(PropertyName = "adult")]
		public bool IsAdult { get; set; }
		[JsonProperty(PropertyName = "overview")]
		public string Overview { get; set; }
		[JsonProperty(PropertyName = "release_date")]
		public DateTime ReleaseDate { get; set; }
		[JsonProperty(PropertyName = "genre_ids")]
		public int[] GenreIds { get; set; }
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }
		[JsonProperty(PropertyName = "original_title")]
		public string OriginalTitle { get; set; }
		[JsonProperty(PropertyName = "original_language")]
		public string OriginalLanguage { get; set; }
		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }
		[JsonProperty(PropertyName = "backdrop_path")]
		public string BackdropImagePath { get; set; }
		[JsonProperty(PropertyName = "popularity")]
		public double Popularity { get; set; }
		[JsonProperty(PropertyName = "vote_count")]
		public int VoteCount { get; set; }
		[JsonProperty(PropertyName = "video")]
		public bool IsVideo { get; set; }
		[JsonProperty(PropertyName = "vote_average")]
		public double VoteAverage { get; set; }
	}
}
