using System;
using System.Collections.Generic;

namespace MovieExplorer.Core {
	public static class MovieApi {
		public const string BaseRequestUrl = "https://api.themoviedb.org/3/";
		public static readonly KeyValuePair<string, string> ApiKeyValueParameter =
			new KeyValuePair<string, string>("api_key", "ab41356b33d100ec61e6c098ecc92140");
		public const string PageKeyParameter = "page";
		public const string LanguageKeyParameter = "language";
		public const string DefaultLanguage = "en-US";

		// API Paths
		public const string ConfigurationPath = "configuration";
		public const string TopRatedMoviesPath = "movie/top_rated";
		public const string UpcomingMoviesPath = "movie/upcoming";
		public const string PopularMoviesPath = "movie/popular";
		public const string NowPlayingMoviesPath = "movie/now_playing";
		public const string LatestMoviesPath = "movie/latest";
	}
}
