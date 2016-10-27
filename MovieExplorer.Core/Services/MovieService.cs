using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieExplorer.Core {
	public class MovieService {

		public enum CollectionType {
			TopRated,
			Upcoming,
			Popular,
			NowPlaying,
			Similar
		}

		static MovieService instance;
		public static MovieService Instance {
			get {
				if (instance == null) {
					throw new Exception(Core.Values.Errors.MovieServiceNotInitialized);
				}
				return instance;
			}
		}

		public Configuration Configuration { get; private set; }

		public FavoriteMoviesDataAccess FavoriteMovies {
			get {
				return FavoriteMoviesDataAccess.Instance;
			}
		}

		private MovieService() { }

		public static void Initialize(ISQLite sqliteService) {
			instance = new MovieService();
			FavoriteMoviesDataAccess.Initialize(sqliteService);
		}

		public async Task<Configuration> UpdateConfigurationAsync() {
			var result = await RequestService.Instance.GetObjectAsync<Configuration>
			                                 (Values.MovieApi.ConfigurationPath)
			                                 .ConfigureAwait(false);
			if (result != null) {
				Configuration = result;
			}
			return result;
		}

		async Task<Movies> GetMoviesAsync(string path, int page, string language) {
			var parameters = new Dictionary<string, string> {
				{ Values.MovieApi.PageKeyParameter, page.ToString() },
				{ Values.MovieApi.LanguageKeyParameter, language }
			};
			var result = await RequestService.Instance.GetObjectAsync<Movies>
			                                 (path, parameters)
			                                 .ConfigureAwait(false);
			return result;
		}

		public async Task<Movies> GetTopRatedMoviesAsync(
			int page = 1, string language = Values.MovieApi.DefaultLanguage) {
			return await GetMoviesAsync(Values.MovieApi.TopRatedMoviesPath, page, language);
		}

		public async Task<Movies> GetUpcomingMoviesAsync(
			int page = 1, string language = Values.MovieApi.DefaultLanguage) {
			return await GetMoviesAsync(Values.MovieApi.UpcomingMoviesPath, page, language);
		}

		public async Task<Movies> GetPopularMoviesAsync(
			int page = 1, string language = Values.MovieApi.DefaultLanguage) {
			return await GetMoviesAsync(Values.MovieApi.PopularMoviesPath, page, language);
		}

		public async Task<Movies> GetNowPlayingMoviesAsync(
			int page = 1, string language = Values.MovieApi.DefaultLanguage) {
			return await GetMoviesAsync(Values.MovieApi.NowPlayingMoviesPath, page, language);
		}

		public async Task<Movies> GetSimilarMoviesAsync(
			int movieId, int page = 1, string language = Values.MovieApi.DefaultLanguage) {
			return await GetMoviesAsync(Values.MovieApi.GetSimilarMoviesPath(movieId), page, language);
		}

		public async Task<Videos> GetVideosAsync(
			int movieId, string language = Values.MovieApi.DefaultLanguage) {
			var parameters = new Dictionary<string, string> {
				{ Values.MovieApi.LanguageKeyParameter, language },
			};
			var result = await RequestService.Instance.GetObjectAsync<Videos>
			                                 (Values.MovieApi.GetVideosPath(movieId), parameters)
			                                 .ConfigureAwait(false);
			return result;
		}
	}
}
