using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MovieExplorer.Core;
using SDWebImage;
using UIKit;
using System.Linq;
using MovieExplorer.Core.Values;

namespace MovieExplorer.iOS {
	public class MovieDetailsPageView : BasePageView {

		public event EventHandler PlayMovieClicked;
		public event EventHandler FavoritesClicked;
		public event EventHandler<Movie> MovieSelected;

		UIImageView backgroundImageView;
		UIVisualEffectView blurEffectView;
		UIScrollView scrollView;
		UIImageView posterImageView;
		UILabel titleLabel;
		UILabel releaseDateLabel;
		UILabel starRatingLabel;
		UILabel voteCountLabel;
		UIButton playVideoButton;
		UIButton favoritesButton;
		UILabel descriptionLabel;
		UILabel similarMoviesLabel;
		MovieCollectionView similarMoviesCollectionView;

		public MovieDetailsPageView(Movie movie) {
			BackgroundColor = Colors.White.AsUIColor();
			var imageUrl = UrlHelper.AppendPath(
				MovieService.Instance.Configuration.Images.SecureBaseUrl,
				MovieService.Instance.Configuration.Images.PosterSizes.ElementAtOrDefault(3));
			imageUrl = UrlHelper.AppendPath(imageUrl, movie.PosterImagePath);

			backgroundImageView = new UIImageView {
				ContentMode = UIViewContentMode.ScaleToFill,
			};
			backgroundImageView.Alpha = .7f;
			backgroundImageView.SetImage(new NSUrl(imageUrl), placeholder: null,
				options: (SDWebImageOptions.ProgressiveDownload | SDWebImageOptions.RetryFailed));
			ContentView.AddSubview(backgroundImageView);

			var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Dark);
			blurEffectView = new UIVisualEffectView(effect: blurEffect);
			ContentView.AddSubview(blurEffectView);

			scrollView = new UIScrollView() {
				BackgroundColor = UIColor.Clear,
				ShowsVerticalScrollIndicator = false
			};
			ContentView.AddSubview(scrollView);

			posterImageView = new UIImageView {
				ContentMode = UIViewContentMode.ScaleAspectFit
			};
			posterImageView.Layer.BorderWidth = 2f;
			posterImageView.Layer.BorderColor = Colors.White.AsUIColor().CGColor;
			posterImageView.SetImage(new NSUrl(imageUrl), placeholder: null,
				options: (SDWebImageOptions.ProgressiveDownload | SDWebImageOptions.RetryFailed));
			scrollView.Add(posterImageView);

			titleLabel = new UILabel {
				Text = movie.Title,
				TextColor = Colors.White.AsUIColor(),
				Lines = 2,
				LineBreakMode = UILineBreakMode.TailTruncation,
				Font = UIFont.FromName(Core.Values.Views.General.HelveticaFontBold, 18f),
				AdjustsFontSizeToFitWidth = true,
			};
			scrollView.Add(titleLabel);

			releaseDateLabel = new UILabel {
				Text = Core.Values.Views.MovieDetailsPageView.GetReleaseDate(movie.ReleaseDate),
				TextColor = Colors.White.AsUIColor(),
				Lines = 1,
				Font = UIFont.FromName(Core.Values.Views.General.HelveticaFont, 14f),
			};
			scrollView.Add(releaseDateLabel);

			starRatingLabel = new UILabel {
				Text = Core.Values.Views.MovieDetailsPageView.GetRatingIcons(movie.VoteAverage),
				TextColor = Colors.Rust.AsUIColor(),
				Font = UIFont.FromName(Core.Values.IconFont.Name, 25f)
			};

			scrollView.Add(starRatingLabel);

			voteCountLabel = new UILabel {
				Text = Core.Values.Views.MovieDetailsPageView.GetVoteCount(movie.VoteCount),
				TextColor = Colors.White.AsUIColor(),
				Font = UIFont.FromName(Core.Values.Views.General.HelveticaFont, 12f)
			};
			scrollView.Add(voteCountLabel);

			playVideoButton = new UIButton {
				BackgroundColor = Colors.Green.AsUIColor(),
			};
			playVideoButton.SetTitle(Core.Values.Views.MovieDetailsPageView.PlayVideo, UIControlState.Normal);
			playVideoButton.SetTitleColor(Colors.White.AsUIColor(), UIControlState.Normal);
			playVideoButton.SetTitleColor(UIColor.Gray, UIControlState.Highlighted);
			playVideoButton.TitleLabel.Font = UIFont.FromName(Core.Values.Views.General.HelveticaFontBold, 15f);
			playVideoButton.TitleLabel.Lines = 1;
			playVideoButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
			playVideoButton.TitleLabel.LineBreakMode = UILineBreakMode.Clip;
			scrollView.Add(playVideoButton);

			favoritesButton = new UIButton {
				BackgroundColor = Colors.Gold.AsUIColor()
			};
			favoritesButton.SetTitleColor(Colors.White.AsUIColor(), UIControlState.Normal);
			favoritesButton.SetTitleColor(UIColor.Gray, UIControlState.Highlighted);
			favoritesButton.TitleLabel.Font = UIFont.FromName(Core.Values.Views.General.HelveticaFontBold, 15f);
			favoritesButton.TitleLabel.Lines = 1;
			favoritesButton.TitleLabel.AdjustsFontSizeToFitWidth = true;
			favoritesButton.TitleLabel.LineBreakMode = UILineBreakMode.Clip;
			favoritesButton.TitleEdgeInsets = new UIEdgeInsets(0f, 10f, 0f, 10f);
			scrollView.Add(favoritesButton);

			descriptionLabel = new UILabel {
				Text = movie.Overview,
				TextColor = Colors.White.AsUIColor(),
				Lines = 0,
				LineBreakMode = UILineBreakMode.WordWrap,
				Font = UIFont.FromName(Core.Values.Views.General.HelveticaFont, 15f)
			};
			scrollView.Add(descriptionLabel);

			similarMoviesLabel = new UILabel() {
				Text = Core.Values.Views.MovieLibraryPageView.SimilarCollectionTitle,
				TextColor = Colors.White.AsUIColor(),
				Font = UIFont.FromName(Core.Values.Views.General.HelveticaFontBold, 16f),
				Hidden = true,
			};
			scrollView.Add(similarMoviesLabel);

			similarMoviesCollectionView = new MovieCollectionView(
				CGRect.Empty,
				new LinearCollectionViewLayout());
			similarMoviesCollectionView.Hidden = true;
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
			var smallMargin = Core.Values.Views.General.SmallMargin;
			var margin = Core.Values.Views.General.DefaultMargin;

			backgroundImageView.Frame = new CGRect(CGPoint.Empty, ContentView.Frame.Size);
			blurEffectView.Frame = backgroundImageView.Frame;

			scrollView.Frame = new CGRect(
				margin, 0f, ContentView.Frame.Width - (margin * 2), ContentView.Frame.Height);

			var currentY = margin;
			var currentX = 0f;

			float posterImageWidth = 
				(posterImageWidth = (float)scrollView.Frame.Width * .45f - margin)
				< 300f ? posterImageWidth : 300f;
			posterImageView.Frame = new CGRect(currentX,currentY,posterImageWidth,posterImageWidth * 1.5f);
			currentX = (float)posterImageView.Frame.Right + margin;

			titleLabel.Frame = new CGRect(currentX,currentY,scrollView.Frame.Width - currentX, 70f);
			titleLabel.SizeToFit();
			currentY = (float)titleLabel.Frame.Bottom + smallMargin;

			releaseDateLabel.Frame = new CGRect(currentX, currentY, scrollView.Frame.Width - currentX, 30f);
			releaseDateLabel.SizeToFit();
			currentY = (float)releaseDateLabel.Frame.Bottom + smallMargin;

			starRatingLabel.Frame = new CGRect(currentX, currentY, scrollView.Frame.Width - currentX, 30f);
			starRatingLabel.SizeToFit();
			currentY = (float)starRatingLabel.Frame.Bottom;

			voteCountLabel.Frame = new CGRect(currentX, currentY, scrollView.Frame.Width - currentX, 20f);
			voteCountLabel.SizeToFit();
			float dynamicMargin = 
				(dynamicMargin = ((float)posterImageView.Frame.Bottom - (float)voteCountLabel.Frame.Bottom) - 90f)
				> smallMargin ? dynamicMargin : smallMargin;
			currentY = (float)voteCountLabel.Frame.Bottom + dynamicMargin;

			playVideoButton.Frame = new CGRect(currentX, currentY, 100f, 40f);
			currentY = (float)playVideoButton.Frame.Bottom + smallMargin;

			favoritesButton.Frame = new CGRect(currentX, currentY, 150f, 40f);
			currentY = Math.Max((float)posterImageView.Frame.Bottom + smallMargin,
			                    (float)favoritesButton.Frame.Bottom + 10f);
			currentX = 0f;

			descriptionLabel.Frame = new CGRect(currentX, currentY, scrollView.Frame.Width, 200f);
			descriptionLabel.SizeToFit();
			dynamicMargin = 
				(dynamicMargin = ((float)Frame.Height - (float)descriptionLabel.Frame.Bottom) - 270f)
				> smallMargin ? dynamicMargin : smallMargin;
			currentY = (float)descriptionLabel.Frame.Bottom + dynamicMargin;

			similarMoviesLabel.Frame = new CGRect(currentX, currentY, scrollView.Frame.Width, 20f);
			currentY = (float)similarMoviesLabel.Frame.Bottom + smallMargin;

			similarMoviesCollectionView.Frame = new CGRect(currentX, currentY, scrollView.Frame.Width, 180f);
			currentY = (float)similarMoviesCollectionView.Frame.Bottom;

			scrollView.ContentSize = new CGSize(scrollView.Frame.Width, currentY);
		}

		public void UpdateSimilarMovies(List<Movie> movies) {
			if (movies != null && movies.Count > 0 && similarMoviesCollectionView != null) {
				similarMoviesLabel.Hidden = false;
				similarMoviesCollectionView.Hidden = false;
				similarMoviesCollectionView.Update(movies);
			}
		}

		public void UpdateFavoritesButton(bool isInFavorites) {
			favoritesButton.SetTitle(isInFavorites ? 
			                         Core.Values.Views.MovieDetailsPageView.RemoveFromFavorites : 
			                         Core.Values.Views.MovieDetailsPageView.SaveToFavorites, UIControlState.Normal);
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
