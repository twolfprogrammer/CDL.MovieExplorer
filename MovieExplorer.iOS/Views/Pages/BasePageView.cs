using System;
using CoreGraphics;
using Foundation;
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
			HeaderView = new HeaderView();
			AddSubview(HeaderView);

			ContentView = new UIView {
				BackgroundColor = UIColor.Magenta,
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
			HeaderView.Frame = new CGRect(0, 0, Frame.Width, 60f);
			ContentView.Frame = new CGRect(5f, HeaderView.Frame.Bottom, Frame.Width - 10f, Frame.Height - HeaderView.Frame.Height);
		}

		void OnBackButtonClicked(object sender, EventArgs e) {
			BackButtonClicked(sender, e);
		}
	}
}
