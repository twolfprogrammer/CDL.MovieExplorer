using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MovieExplorer.Core;
using SDWebImage;
using UIKit;
using System.Linq;

namespace MovieExplorer.iOS {
	public class MovieDetailsPageView : BasePageView {

		public event EventHandler PlayMovieClicked;
		public event EventHandler FavoritesClicked;
		public event EventHandler<Movie> MovieSelected;

		UIScrollView scrollView;
		UIImageView posterImageView;
		UILabel titleLabel;
		UILabel releaseDateLabel;
		UILabel starRatingLabel;
		UILabel voteCountLabel;
		UIButton playVideoButton;
		UIButton favoritesButton;
		UILabel descriptionLabel;
		MovieCollectionView similarMoviesCollectionView;

		public MovieDetailsPageView(Movie movie) {
			BackgroundColor = UIColor.Cyan;

			scrollView = new UIScrollView() {
				BackgroundColor = UIColor.Orange
			};
			ContentView.AddSubview(scrollView);

			posterImageView = new UIImageView {
				BackgroundColor = UIColor.LightGray,
			};
			var imageUrl = UrlHelper.AppendPath(
				MovieService.Instance.Configuration.Images.SecureBaseUrl,
				MovieService.Instance.Configuration.Images.PosterSizes.ElementAtOrDefault(3));
			imageUrl = UrlHelper.AppendPath(imageUrl, movie.PosterImagePath);
			posterImageView.SetImage(new NSUrl(imageUrl), placeholder: null,
				options: (SDWebImageOptions.ProgressiveDownload | SDWebImageOptions.RetryFailed));
			scrollView.Add(posterImageView);

			titleLabel = new UILabel {
				Text = movie.Title,
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.Red,
				Lines = 0,
				LineBreakMode = UILineBreakMode.WordWrap,
				AdjustsFontSizeToFitWidth = true,
			};
			scrollView.Add(titleLabel);

			releaseDateLabel = new UILabel {
				Text = Core.Values.Views.MovieDetailsPageView.GetReleaseDate(movie.ReleaseDate),
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.Purple,
			};
			scrollView.Add(releaseDateLabel);

			starRatingLabel = new UILabel {
				Text = Core.Values.Views.MovieDetailsPageView.GetRatingIcons(movie.VoteAverage),
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.Magenta,
				Font = UIFont.FromName(Core.Values.IconFont.Name, 25)
			};
			scrollView.Add(starRatingLabel);

			voteCountLabel = new UILabel {
				Text = Core.Values.Views.MovieDetailsPageView.GetVoteCount(movie.VoteCount),
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.Blue,
			};
			scrollView.Add(voteCountLabel);

			playVideoButton = new UIButton {
				BackgroundColor = UIColor.Green
			};
			playVideoButton.SetTitle(Core.Values.Views.MovieDetailsPageView.PlayVideo, UIControlState.Normal);
			playVideoButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			playVideoButton.SetTitleColor(UIColor.Gray, UIControlState.Highlighted);
			scrollView.Add(playVideoButton);

			favoritesButton = new UIButton {
				BackgroundColor = Core.Values.Colors.Test.AsUIColor()
			};
			favoritesButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			favoritesButton.SetTitleColor(UIColor.Gray, UIControlState.Highlighted);
			scrollView.Add(favoritesButton);

			descriptionLabel = new UILabel {
				Text = movie.Overview,
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.Red,
				Lines = 0,
				LineBreakMode = UILineBreakMode.WordWrap
			};
			scrollView.Add(descriptionLabel);

			similarMoviesCollectionView = new MovieCollectionView(
				CGRect.Empty,
				new LinearCollectionViewLayout());
			scrollView.Add(similarMoviesCollectionView);
			playVideoButton.TouchUpInside += OnPlayVideoClicked;
			favoritesButton.TouchUpInside += OnFavoritesClicked;
			similarMoviesCollectionView.MovieSelected += OnMovieSelected;
		}

		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			playVideoButton.TouchUpInside -= OnPlayVideoClicked;
			favoritesButton.TouchUpInside -= OnFavoritesClicked;
			similarMoviesCollectionView.MovieSelected -= OnMovieSelected;
		}

		public override void LayoutSubviews() {
			base.LayoutSubviews();
			scrollView.Frame = new CGRect(0f,0f, ContentView.Frame.Width, ContentView.Frame.Height);
			posterImageView.Frame = new CGRect(0f,0f,180f,270);
			titleLabel.Frame = new CGRect(posterImageView.Frame.Right,0,scrollView.Frame.Width - posterImageView.Frame.Right,70f);
			releaseDateLabel.Frame = new CGRect(posterImageView.Frame.Right, titleLabel.Frame.Bottom, scrollView.Frame.Width - posterImageView.Frame.Right, 40f);
			starRatingLabel.Frame = new CGRect(posterImageView.Frame.Right, releaseDateLabel.Frame.Bottom, scrollView.Frame.Width - posterImageView.Frame.Right, 40f);
			voteCountLabel.Frame = new CGRect(posterImageView.Frame.Right, starRatingLabel.Frame.Bottom, scrollView.Frame.Width - posterImageView.Frame.Right, 40f);
			playVideoButton.Frame = new CGRect(posterImageView.Frame.Right, voteCountLabel.Frame.Bottom, scrollView.Frame.Width - posterImageView.Frame.Right, 40f);
			favoritesButton.Frame = new CGRect(posterImageView.Frame.Right, playVideoButton.Frame.Bottom, scrollView.Frame.Width - posterImageView.Frame.Right, 40f);
			descriptionLabel.Frame = new CGRect(0, posterImageView.Frame.Bottom, scrollView.Frame.Width, 200f);
			similarMoviesCollectionView.Frame = new CGRect(0, descriptionLabel.Frame.Bottom, scrollView.Frame.Width, 200f);
			scrollView.ContentSize = new CGSize(ContentView.Frame.Width, similarMoviesCollectionView.Frame.Bottom);
		}

		public void UpdateSimilarMovies(List<Movie> movies) {
			if (movies != null && movies.Count > 0 && similarMoviesCollectionView != null) {
				similarMoviesCollectionView.Update(movies);
			}
		}

		public void UpdateFavoritesButton(bool isInFavorites) {
			favoritesButton.SetTitle(isInFavorites ? 
			                         Core.Values.Views.MovieDetailsPageView.RemoveFromFavorites : 
			                         Core.Values.Views.MovieDetailsPageView.AddToFavorites, UIControlState.Normal);
		}

		void OnPlayVideoClicked(object sender, EventArgs e) {
			PlayMovieClicked(sender, e);
		}

		void OnFavoritesClicked(object sender, EventArgs e) {
			FavoritesClicked(sender, e);
		}

		void OnMovieSelected(object sender, Movie e) {
			MovieSelected(sender, e);
		}
	}
}
