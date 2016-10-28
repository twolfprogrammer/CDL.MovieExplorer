using System;
using System.Collections.Generic;
using CoreGraphics;
using MovieExplorer.Core;
using UIKit;

namespace MovieExplorer.iOS {
	public class MovieCollectionView : UICollectionView {

		public event EventHandler NeedsMoreMovies;
		public event EventHandler<Movie> MovieSelected;

		MovieCollectionViewSource viewSource {
			get {
				return Source as MovieCollectionViewSource;
			}
		}

		public MovieCollectionView(CGRect frame, UICollectionViewLayout layout) : base(frame, layout) {
			BackgroundColor = UIColor.Green;
			RegisterClassForCell(typeof(MovieCollectionViewCell), MovieCollectionViewCell.CellId);
			Source = new MovieCollectionViewSource();
			ShowsVerticalScrollIndicator = false;
			ShowsHorizontalScrollIndicator = false;
			viewSource.MovieSelected += OnMovieSelected;
			viewSource.NeedsMoreMovies += OnNeedsMore;
		}

		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			viewSource.MovieSelected -= OnMovieSelected;
			viewSource.NeedsMoreMovies -= OnNeedsMore;
		}

		public void Update(List<Movie> movies) {
			movies?.RemoveAll(m => string.IsNullOrWhiteSpace(m.PosterImagePath));
			viewSource.Update(movies);
			NSOperationQueue.MainQueue.AddOperation(() => {
				ReloadData();
			});
		}

		void OnMovieSelected(object sender, Movie e) {
			if (MovieSelected != null) {
				MovieSelected(sender, e);
			}
		}

		void OnNeedsMore(object sender, EventArgs e) {
			if (NeedsMoreMovies != null) {
				NeedsMoreMovies(sender, e);
			}
		}
	}
}
