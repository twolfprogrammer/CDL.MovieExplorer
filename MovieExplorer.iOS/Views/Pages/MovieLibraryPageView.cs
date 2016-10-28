using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using MovieExplorer.Core;
using MovieExplorer.Core.Values;
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
			scrollView = new UIScrollView() {
				BackgroundColor = UIColor.Clear,
				ShowsVerticalScrollIndicator = false,
			};
			ContentView.AddSubview(scrollView);

			refreshControl = new UIRefreshControl() {
				TintColor = Colors.Gold.AsUIColor(),
			};
			scrollView.Add(refreshControl);

			topRatedMoviesLabel = new UILabel() {
				Text = Core.Values.Views.MovieLibraryPageView.TopRatedCollectionTitle,
				TextColor = Colors.White.AsUIColor(),
				Font = UIFont.FromName(Core.Values.Views.General.HelveticaFontBold, 16f),
			};
			scrollView.Add(topRatedMoviesLabel);

			topRatedMoviesCollection = new MovieCollectionView(
				CGRect.Empty,
				new LinearCollectionViewLayout());
			scrollView.Add(topRatedMoviesCollection);

			popularMoviesLabel = new UILabel() {
				Text = Core.Values.Views.MovieLibraryPageView.PopularCollectionTitle,
				TextColor = Colors.White.AsUIColor(),
				Font = UIFont.FromName(Core.Values.Views.General.HelveticaFontBold, 16f),
			};
			scrollView.Add(popularMoviesLabel);

			popularMoviesCollection = new MovieCollectionView(
				CGRect.Empty,
				new LinearCollectionViewLayout());
			scrollView.Add(popularMoviesCollection);

			nowPlayingMoviesLabel = new UILabel() {
				Text = Core.Values.Views.MovieLibraryPageView.NowPlayingCollectionTitle,
				TextColor = Colors.White.AsUIColor(),
				Font = UIFont.FromName(Core.Values.Views.General.HelveticaFontBold, 16f),
			};
			scrollView.Add(nowPlayingMoviesLabel);

			nowPlayingMoviesCollection = new MovieCollectionView(
				CGRect.Empty,
				new LinearCollectionViewLayout());
			scrollView.Add(nowPlayingMoviesCollection);

			upcomingMoviesLabel = new UILabel() {
				Text = Core.Values.Views.MovieLibraryPageView.UpcomingCollectionTitle,
				TextColor = Colors.White.AsUIColor(),
				Font = UIFont.FromName(Core.Values.Views.General.HelveticaFontBold, 16f),
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
			var margin = Core.Values.Views.General.DefaultMargin;

			scrollView.Frame = new CGRect(margin, 0f, ContentView.Frame.Width - (margin * 2), ContentView.Frame.Height);
			topRatedMoviesLabel.Frame = new CGRect(0f, Core.Values.Views.General.DefaultMargin, scrollView.Frame.Width, 20f);
			topRatedMoviesCollection.Frame = new CGRect(0f, topRatedMoviesLabel.Frame.Bottom, scrollView.Frame.Width, 180f);
			popularMoviesLabel.Frame = new CGRect(0f, topRatedMoviesCollection.Frame.Bottom + margin, scrollView.Frame.Width, 20f);
			popularMoviesCollection.Frame = new CGRect(0f, popularMoviesLabel.Frame.Bottom, scrollView.Frame.Width, 180f);
			nowPlayingMoviesLabel.Frame = new CGRect(0f, popularMoviesCollection.Frame.Bottom + margin, scrollView.Frame.Width, 20f);
			nowPlayingMoviesCollection.Frame = new CGRect(0f, nowPlayingMoviesLabel.Frame.Bottom, scrollView.Frame.Width, 180f);
			upcomingMoviesLabel.Frame = new CGRect(0f, nowPlayingMoviesCollection.Frame.Bottom + margin, scrollView.Frame.Width, 20f);
			upcomingMoviesCollection.Frame = new CGRect(0f, upcomingMoviesLabel.Frame.Bottom, scrollView.Frame.Width, 180f);
			scrollView.ContentSize = new CGSize(scrollView.Frame.Width, upcomingMoviesCollection.Frame.Bottom);
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
