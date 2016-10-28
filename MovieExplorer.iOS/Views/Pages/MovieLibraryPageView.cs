using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using MovieExplorer.Core;
using UIKit;

namespace MovieExplorer.iOS {
	public class MovieLibraryPageView : BasePageView {

		public event EventHandler RefreshRequested;
		public event EventHandler<MovieService.CollectionType> NeedsMoreMovies;
		public event EventHandler<Movie> MovieSelected;

		UIRefreshControl refreshControl;
		UIScrollView scrollView;
		MovieCollectionView topRatedMoviesCollection;
		MovieCollectionView popularMoviesCollection;
		MovieCollectionView nowPlayingMoviesCollection;
		MovieCollectionView upcomingMoviesCollection;
		UILabel topRatedMoviesLabel;
		UILabel popularMoviesLabel;
		UILabel nowPlayingMoviesLabel;
		UILabel upcomingMoviesLabel;

		public MovieLibraryPageView() {
			BackgroundColor = UIColor.Magenta;

			scrollView = new UIScrollView() {
				BackgroundColor = UIColor.Orange
			};
			ContentView.AddSubview(scrollView);

			refreshControl = new UIRefreshControl();
			scrollView.Add(refreshControl);

			topRatedMoviesLabel = new UILabel() {
				Text = Core.Values.Views.MovieLibraryPageView.TopRatedCollectionTitle,
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.Blue,
				Font = UIFont.FromName(Core.Values.Views.General.iOSDefaultFont, 20f),
			};
			scrollView.Add(topRatedMoviesLabel);

			topRatedMoviesCollection = new MovieCollectionView(
				CGRect.Empty,
				new LinearCollectionViewLayout());
			scrollView.Add(topRatedMoviesCollection);

			popularMoviesLabel = new UILabel() {
				Text = Core.Values.Views.MovieLibraryPageView.PopularCollectionTitle,
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.Blue,
				Font = UIFont.FromName(Core.Values.Views.General.iOSDefaultFont, 20f),
			};
			scrollView.Add(popularMoviesLabel);

			popularMoviesCollection = new MovieCollectionView(
				CGRect.Empty,
				new LinearCollectionViewLayout());
			scrollView.Add(popularMoviesCollection);

			nowPlayingMoviesLabel = new UILabel() {
				Text = Core.Values.Views.MovieLibraryPageView.NowPlayingCollectionTitle,
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.Blue,
				Font = UIFont.FromName(Core.Values.Views.General.iOSDefaultFont, 20f),
			};
			scrollView.Add(nowPlayingMoviesLabel);

			nowPlayingMoviesCollection = new MovieCollectionView(
				CGRect.Empty,
				new LinearCollectionViewLayout());
			scrollView.Add(nowPlayingMoviesCollection);

			upcomingMoviesLabel = new UILabel() {
				Text = Core.Values.Views.MovieLibraryPageView.UpcomingCollectionTitle,
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.Blue,
				Font = UIFont.FromName(Core.Values.Views.General.iOSDefaultFont, 20f),
			};
			scrollView.Add(upcomingMoviesLabel);

			upcomingMoviesCollection = new MovieCollectionView(
				CGRect.Empty,
				new LinearCollectionViewLayout());
			scrollView.Add(upcomingMoviesCollection);

			refreshControl.ValueChanged += OnRefreshRequested;
			topRatedMoviesCollection.MovieSelected += Collection_MovieSelected;
			popularMoviesCollection.MovieSelected += Collection_MovieSelected;
			nowPlayingMoviesCollection.MovieSelected += Collection_MovieSelected;
			upcomingMoviesCollection.MovieSelected += Collection_MovieSelected;
			topRatedMoviesCollection.NeedsMoreMovies += OnNeedsTopRatedMovies;
			popularMoviesCollection.NeedsMoreMovies += OnNeedsMorePopularMovies;
			nowPlayingMoviesCollection.NeedsMoreMovies += OnNeedsMoreNowPlayingMovies;
			upcomingMoviesCollection.NeedsMoreMovies += OnNeedsMoreUpcomingMovies;
		}

		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			refreshControl.ValueChanged -= OnRefreshRequested;
			topRatedMoviesCollection.MovieSelected -= Collection_MovieSelected;
			popularMoviesCollection.MovieSelected -= Collection_MovieSelected;
			nowPlayingMoviesCollection.MovieSelected -= Collection_MovieSelected;
			upcomingMoviesCollection.MovieSelected -= Collection_MovieSelected;
			topRatedMoviesCollection.NeedsMoreMovies -= OnNeedsTopRatedMovies;
			popularMoviesCollection.NeedsMoreMovies -= OnNeedsMorePopularMovies;
			nowPlayingMoviesCollection.NeedsMoreMovies -= OnNeedsMoreNowPlayingMovies;
			upcomingMoviesCollection.NeedsMoreMovies -= OnNeedsMoreUpcomingMovies;
		}

		public override void LayoutSubviews() {
			base.LayoutSubviews();
			scrollView.Frame = new CGRect(0f, 0f, ContentView.Frame.Width, ContentView.Frame.Height);
			topRatedMoviesLabel.Frame = new CGRect(0, 0, scrollView.Frame.Width, 45f);
			topRatedMoviesCollection.Frame = new CGRect(0, topRatedMoviesLabel.Frame.Bottom, scrollView.Frame.Width, 200);
			popularMoviesLabel.Frame = new CGRect(0, topRatedMoviesCollection.Frame.Bottom, scrollView.Frame.Width, 45f);
			popularMoviesCollection.Frame = new CGRect(0, popularMoviesLabel.Frame.Bottom, scrollView.Frame.Width, 200);
			nowPlayingMoviesLabel.Frame = new CGRect(0, popularMoviesCollection.Frame.Bottom, scrollView.Frame.Width, 45f);
			nowPlayingMoviesCollection.Frame = new CGRect(0, nowPlayingMoviesLabel.Frame.Bottom + 5f, scrollView.Frame.Width, 200);
			upcomingMoviesLabel.Frame = new CGRect(0, nowPlayingMoviesCollection.Frame.Bottom, scrollView.Frame.Width, 45f);
			upcomingMoviesCollection.Frame = new CGRect(0, upcomingMoviesLabel.Frame.Bottom + 5f, scrollView.Frame.Width, 200);
			scrollView.ContentSize = new CGSize(ContentView.Frame.Width, upcomingMoviesCollection.Frame.Bottom);
		}

		public void ShowRefreshing(bool value) {
			if (value) {
				refreshControl.BeginRefreshing();
			}
			else {
				refreshControl.EndRefreshing();
			}
		}

		public void UpdateMovies(MovieService.CollectionType collectionType, List<Movie> movies) {
			if (collectionType == MovieService.CollectionType.TopRated) {
				topRatedMoviesCollection.Update(movies);
			}
			else if (collectionType == MovieService.CollectionType.NowPlaying) {
				nowPlayingMoviesCollection.Update(movies);
			}
			else if(collectionType == MovieService.CollectionType.Popular){
				popularMoviesCollection.Update(movies);
			}
			else if(collectionType == MovieService.CollectionType.Upcoming){
				upcomingMoviesCollection.Update(movies);
			}
		}

		void Collection_MovieSelected(object sender, Movie e) {
			if (MovieSelected != null) {
				MovieSelected(sender, e);
			}
		}

		void OnNeedsTopRatedMovies(object sender, EventArgs e) {
			if (NeedsMoreMovies != null) {
				NeedsMoreMovies(sender, MovieService.CollectionType.TopRated);
			}
		}

		void OnNeedsMorePopularMovies(object sender, EventArgs e) {
			if (NeedsMoreMovies != null) {
				NeedsMoreMovies(sender, MovieService.CollectionType.Popular);
			}
		}

		void OnNeedsMoreNowPlayingMovies(object sender, EventArgs e) {
			if (NeedsMoreMovies != null) {
				NeedsMoreMovies(sender, MovieService.CollectionType.NowPlaying);
			}
		}

		void OnNeedsMoreUpcomingMovies(object sender, EventArgs e) {
			if (NeedsMoreMovies != null) {
				NeedsMoreMovies(sender, MovieService.CollectionType.Upcoming);
			}
		}

		void OnRefreshRequested(object sender, EventArgs e) {
			RefreshRequested(sender, e);
		}
	}
}
