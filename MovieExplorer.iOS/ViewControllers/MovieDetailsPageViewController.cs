using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using MovieExplorer.Core;
using UIKit;

namespace MovieExplorer.iOS {
	public class MovieDetailsPageViewController : BasePageViewController {

		List<Movie> similarMovies = new List<Movie>();
		Movie movie;
		bool isInFavorites;

		MovieDetailsPageView pageView {
			get {
				return ((MovieDetailsPageView)View);
			}
		}

		public MovieDetailsPageViewController(Movie movie) {
			this.movie = movie;
			View = new MovieDetailsPageView(movie);
		}

		public override void ViewWillAppear(bool animated) {
			base.ViewWillAppear(animated);
			LoadSimilarMoviesAsync().ContinueWith((task) => { });
			isInFavorites = MovieService.Instance.FavoriteMovies.ContainsMovie(movie.Id);
			pageView.UpdateFavoritesButton(isInFavorites);
			pageView.MovieSelected += OnMovieSelected;
			pageView.PlayMovieClicked += OnPlayMovieClicked;
			pageView.FavoritesClicked += OnFavoritesClicked;
		}

		public override void ViewWillDisappear(bool animated) {
			base.ViewWillDisappear(animated);
			pageView.MovieSelected -= OnMovieSelected;
			pageView.PlayMovieClicked -= OnPlayMovieClicked;
			pageView.FavoritesClicked -= OnFavoritesClicked;
		}

		async Task LoadSimilarMoviesAsync() {
			var movies = await MovieService.Instance.GetSimilarMoviesAsync(movie.Id);
			if (movies != null) {
				InvokeOnMainThread(() => {
					similarMovies.AddRange(movies.Results);
					pageView.UpdateSimilarMovies(movies.Results);
				});
			}
		}

		void OnPlayMovieClicked(object sender, EventArgs e) {
			MovieService.Instance.GetVideosAsync(movie.Id).ContinueWith((videosTask) => {
				var videos = videosTask.Result;
				if (videos != null) {
					var video = videos.Results.FirstOrDefault(
						v => v.Type == Core.Values.MovieApi.VideoType);
					if (video != null) {
						InvokeOnMainThread(() => {
							UIApplication.SharedApplication.OpenUrl(
								new NSUrl(Core.Values.MovieApi.GetVideoUrl(video.Key)));
						});
					}
				}
			});
		}

		void OnFavoritesClicked(object sender, EventArgs e) {
			if (isInFavorites) {
				isInFavorites = !MovieService.Instance.FavoriteMovies.RemoveMovie(movie.Id);
			}
			else {
				isInFavorites = MovieService.Instance.FavoriteMovies.SaveMovie(movie.Id, movie.Title);
			}
			pageView.UpdateFavoritesButton(isInFavorites);
		}

		void OnMovieSelected(object sender, Movie e) {
			NavigationController?.PushViewController(new MovieDetailsPageViewController(e), animated: true);
		}
	}
}
