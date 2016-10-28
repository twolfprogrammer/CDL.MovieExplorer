using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using MovieExplorer.Core;
using SDWebImage;
using UIKit;

namespace MovieExplorer.iOS {
public class MovieCollectionViewSource : UICollectionViewSource {

		public event EventHandler NeedsMoreMovies;
		public event EventHandler<Movie> MovieSelected;

		List<Movie> movies = new List<Movie>();

		public MovieCollectionViewSource() { }

		public void Update(List<Movie> movies) {
			if (movies == null) {
				this.movies.Clear();
			}
			else {
			this.movies.AddRange(movies);
		}
		}

		public override nint NumberOfSections(UICollectionView collectionView) {
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section) {
			return movies.Count;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath) {
			var cell = collectionView.DequeueReusableCell(MovieCollectionViewCell.CellId, indexPath) as MovieCollectionViewCell;
			var imageUrl = UrlHelper.AppendPath(
				MovieService.Instance.Configuration.Images.SecureBaseUrl,
				MovieService.Instance.Configuration.Images.PosterSizes.ElementAtOrDefault(2));
			imageUrl = UrlHelper.AppendPath(imageUrl, movies[indexPath.Row].PosterImagePath);
			cell.ImageView.SetImage(new NSUrl(imageUrl), placeholder: null,
				options: (SDWebImageOptions.ProgressiveDownload | SDWebImageOptions.RetryFailed));
			return cell;
		}

		public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath) {
			var cell = collectionView.CellForItem(indexPath);
			cell.ContentView.Layer.BorderColor = UIColor.Yellow.CGColor;
		}

		public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath) {
			var cell = collectionView.CellForItem(indexPath);
			cell.ContentView.Layer.BorderColor = UIColor.White.CGColor;
		}

		public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath) {
			return true;
		}

		public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath) {
			if (MovieSelected != null) {
				MovieSelected(this, movies[indexPath.Row]);
			}
		}

		public override bool ShouldSelectItem(UICollectionView collectionView, NSIndexPath indexPath) {
			return true;
		}

		public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath) {
			if (indexPath.Row == movies.Count - 10 && NeedsMoreMovies != null) {
				NeedsMoreMovies(this, EventArgs.Empty);
			}
		}
	}
}
