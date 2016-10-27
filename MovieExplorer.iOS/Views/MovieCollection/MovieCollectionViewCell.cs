using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MovieExplorer.iOS {
	public class MovieCollectionViewCell : UICollectionViewCell {

		public static NSString CellId = new NSString("MovieCell");

		public UIImageView ImageView { get; private set; }

		[Export("initWithFrame:")]
		public MovieCollectionViewCell(CGRect frame) : base(frame) {
			ContentView.Layer.BorderColor = UIColor.White.CGColor;
			ContentView.Layer.BorderWidth = 2.0f;
			ContentView.BackgroundColor = UIColor.White;
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
