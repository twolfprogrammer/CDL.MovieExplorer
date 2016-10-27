using System;
using UIKit;
using MovieExplorer.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieExplorer.iOS {
	public class MovieLibraryPageViewController : BasePageViewController {

		List<Movie> topRatedMovies = new List<Movie>();
		List<Movie> popularMovies = new List<Movie>();
		List<Movie> nowPlayingMovies = new List<Movie>();
		List<Movie> upcomingMovies = new List<Movie>();

		MovieLibraryPageView pageView {
			get {
				return View as MovieLibraryPageView;
			}
		}

		public MovieLibraryPageViewController() {
			View = new MovieLibraryPageView();
		}

		public override void ViewWillAppear(bool animated) {
			base.ViewWillAppear(animated);
			SetupAsync().ContinueWith((task) => { });
			pageView.RefreshRequested += OnRefreshRequested;
			pageView.MovieSelected += OnMovieSelected;
			pageView.NeedsMoreMovies += OnNeedsMoreMovies;
		}

		public override void ViewWillDisappear(bool animated) {
			base.ViewWillDisappear(animated);
			pageView.RefreshRequested -= OnRefreshRequested;
			pageView.MovieSelected -= OnMovieSelected;
			pageView.NeedsMoreMovies -= OnNeedsMoreMovies;
		}

		async Task SetupAsync() {
			await LoadConfigurationAsync();
			await LoadTopRatedMoviesAsync();
			await LoadPopularMoviesAsync();
			await LoadNowPlayingMoviesAsync();
			await LoadUpComingMoviesAsync();
		}

		async Task<bool> LoadConfigurationAsync() {
			if (MovieService.Instance.Configuration == null) {
				await MovieService.Instance.UpdateConfigurationAsync();
			}
			return MovieService.Instance.Configuration != null;
		}

		int topRatedMoviesPageIndex = 1;
		async Task LoadTopRatedMoviesAsync() {
			var movies = await MovieService.Instance.GetTopRatedMoviesAsync(topRatedMoviesPageIndex);
			if (movies != null) {
				InvokeOnMainThread(() => {
					topRatedMoviesPageIndex++;
					topRatedMovies.AddRange(movies.Results);
					pageView.UpdateMovies(MovieService.CollectionType.TopRated, movies.Results);
				});
			}
		}

		int popularMoviesPageIndex = 1;
		async Task LoadPopularMoviesAsync() {
			var movies = await MovieService.Instance.GetPopularMoviesAsync(popularMoviesPageIndex);
			if (movies != null) {
				InvokeOnMainThread(() => {
					popularMoviesPageIndex++;
					popularMovies.AddRange(movies.Results);
					pageView.UpdateMovies(MovieService.CollectionType.Popular, movies.Results);
				});
			}
		}

		int nowPlayingMoviesPageIndex = 1;
		async Task LoadNowPlayingMoviesAsync() {
			var movies = await MovieService.Instance.GetNowPlayingMoviesAsync(nowPlayingMoviesPageIndex);
			if (movies != null) {
				InvokeOnMainThread(() => {
					nowPlayingMoviesPageIndex++;
					nowPlayingMovies.AddRange(movies.Results);
					pageView.UpdateMovies(MovieService.CollectionType.NowPlaying, movies.Results);
				});
			}
		}

		int upcomingMoviesPageIndex = 1;
		async Task LoadUpComingMoviesAsync() {
			var movies = await MovieService.Instance.GetUpcomingMoviesAsync(upcomingMoviesPageIndex);
			if (movies != null) {
				InvokeOnMainThread(() => {
					upcomingMoviesPageIndex++;
					upcomingMovies.AddRange(movies.Results);
					pageView.UpdateMovies(MovieService.CollectionType.Upcoming, movies.Results);
				});
			}
		}

		void OnRefreshRequested(object sender, EventArgs e) {
			pageView.ShowRefreshing(true);
			SetupAsync().ContinueWith((task) => {
				InvokeOnMainThread(() => {
					pageView.ShowRefreshing(false);
				});
			});
		}

		void OnMovieSelected(object sender, Movie e) {
			NavigationController?.PushViewController(new MovieDetailsPageViewController(e), animated: true);
		}

		void OnNeedsMoreMovies(object sender, MovieService.CollectionType e) {
			if (e == MovieService.CollectionType.TopRated) {
				LoadTopRatedMoviesAsync().ContinueWith((task) => { });
			}
			else if (e == MovieService.CollectionType.Popular) {
				LoadPopularMoviesAsync().ContinueWith((task) => { });
			}
			else if (e == MovieService.CollectionType.NowPlaying) {
				LoadNowPlayingMoviesAsync().ContinueWith((task) => { });
			}
			else if (e == MovieService.CollectionType.Upcoming) {
				LoadUpComingMoviesAsync().ContinueWith((task) => { });
			}
		}
	}
}
