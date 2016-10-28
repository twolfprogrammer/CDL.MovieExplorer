using System;
using CoreGraphics;
using Foundation;
using MovieExplorer.Core.Values;
using UIKit;

namespace MovieExplorer.iOS {
	public abstract class BasePageView : UIView {

		public event EventHandler BackButtonClicked;

		protected HeaderView HeaderView { get; private set; }
		protected UIView ContentView { get; private set; }

		public bool ShowBackButton {
			get {
				return HeaderView.ShowBackButton;
			}
			set {
				HeaderView.ShowBackButton = value;
			}
		}

		public BasePageView() {
			BackgroundColor = Colors.Gray.AsUIColor();
			HeaderView = new HeaderView();
			AddSubview(HeaderView);

			ContentView = new UIView {
				BackgroundColor = UIColor.Clear,
			};
			AddSubview(ContentView);

			HeaderView.BackButtonClicked += OnBackButtonClicked;
		}

		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			HeaderView.BackButtonClicked -= OnBackButtonClicked;
		}

		public override void LayoutSubviews() {
			base.LayoutSubviews();
			HeaderView.Frame = new CGRect(0f, 0f, Frame.Width, 45f);
			ContentView.Frame = new CGRect(
				0f, HeaderView.Frame.Bottom,Frame.Width,
				Frame.Height - HeaderView.Frame.Height);
		}

		void OnBackButtonClicked(object sender, EventArgs e) {
			BackButtonClicked(sender, e);
		}
	}
}
