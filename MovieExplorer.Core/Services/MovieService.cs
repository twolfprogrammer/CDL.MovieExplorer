using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieExplorer.Core {
	public class MovieService {

		RequestService requestService;

		public MovieService() {
			requestService = new RequestService();
		}

		public async Task<Configuration> GetConfigurationAsync() {
			var result = await requestService.GetObjectAsync<Configuration>
			                                 (MovieApi.ConfigurationPath, null).ConfigureAwait(false);
			return result;
		}

		async Task<Movies> GetMoviesAsync(string path, int page, string language) {
			var parameters = new Dictionary<string, string> {
				{ MovieApi.PageKeyParameter, page.ToString() },
				{ MovieApi.LanguageKeyParameter, language }
			};
			var result = await requestService.GetObjectAsync<Movies>
			                                 (path, parameters).ConfigureAwait(false);
			return result;
		}

		public async Task<Movies> GetTopRatedMoviesAsync(int page = 1, string language = MovieApi.DefaultLanguage) {
			return await GetMoviesAsync(MovieApi.TopRatedMoviesPath, page, language);
		}

		public async Task<Movies> GetUpcomingMoviesAsync(int page = 1, string language = MovieApi.DefaultLanguage) {
			return await GetMoviesAsync(MovieApi.UpcomingMoviesPath, page, language);
		}

		public async Task<Movies> GetPopularMoviesAsync(int page = 1, string language = MovieApi.DefaultLanguage) {
			return await GetMoviesAsync(MovieApi.PopularMoviesPath, page, language);
		}

		public async Task<Movies> GetLatestMoviesAsync(int page = 1, string language = MovieApi.DefaultLanguage) {
			return await GetMoviesAsync(MovieApi.LatestMoviesPath, page, language); //TODO: not working, fix
		}

		public async Task<Movies> GetNowPlayingMoviesAsync(int page = 1, string language = MovieApi.DefaultLanguage) {
			return await GetMoviesAsync(MovieApi.NowPlayingMoviesPath, page, language);
		}
	}
}
