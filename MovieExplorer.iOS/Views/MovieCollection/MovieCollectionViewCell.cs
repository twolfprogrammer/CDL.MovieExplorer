using System;
using CoreGraphics;
using Foundation;
using MovieExplorer.Core.Values;
using UIKit;

namespace MovieExplorer.iOS {
	public class MovieCollectionViewCell : UICollectionViewCell {

		public static NSString CellId = new NSString("MovieCell");

		public UIImageView ImageView { get; private set; }

		[Export("initWithFrame:")]
		public MovieCollectionViewCell(CGRect frame) : base(frame) {
			ContentView.Layer.BorderColor = Colors.White.AsUIColor().CGColor;
			ContentView.Layer.BorderWidth = 2f;
			ContentView.BackgroundColor = Colors.White.AsUIColor();
			ImageView = new UIImageView();
			ContentView.AddSubview(ImageView);
		}

		public override void LayoutSubviews() {
			base.LayoutSubviews();
			ImageView.Frame = ContentView.Frame;
			ImageView.Center = ContentView.Center;
			ImageView.ContentMode = UIViewContentMode.ScaleToFill;
		}
	}
}
