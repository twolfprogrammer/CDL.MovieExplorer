using System;
using CoreGraphics;
using UIKit;

namespace MovieExplorer.iOS {
	public class HeaderView : UIView {

		public event EventHandler BackButtonClicked;

		UILabel titleLabel;
		UIButton backButton;

		private bool showBackButton = false;
		public bool ShowBackButton {
			get {
				return showBackButton;
			}
			set {
				if (backButton != null) {
					backButton.Hidden = !value;
				}
				showBackButton = value;
			}
		}

		public HeaderView() {
			BackgroundColor = UIColor.LightGray;
			titleLabel = new UILabel {
				Text = Core.Values.Views.General.HeaderTitle,
				TextColor = UIColor.Black,
				BackgroundColor = UIColor.DarkGray,
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.FromName(Core.Values.Views.General.iOSDefaultFont, 20f),
			};
			AddSubview(titleLabel);

			backButton = new UIButton {
				BackgroundColor = UIColor.Clear,
				Font = UIFont.FromName(Core.Values.IconFont.Name, 25),
			};
			backButton.SetTitle(Core.Values.IconFont.IconClose, UIControlState.Normal);
			backButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			backButton.SetTitleColor(UIColor.Gray, UIControlState.Highlighted);
			backButton.Hidden = !showBackButton;
			AddSubview(backButton);

			backButton.TouchUpInside += OnBackButtonClicked;
		}

		protected override void Dispose(bool disposing) {
			base.Dispose(disposing);
			backButton.TouchUpInside -= OnBackButtonClicked;
		}

		public override void LayoutSubviews() {
			base.LayoutSubviews();
			titleLabel.Frame = new CGRect(CGPoint.Empty, new CGSize(200f, Frame.Height));
			titleLabel.Center = this.Center;
			backButton.Frame = new CGRect(Frame.Width - Frame.Height, 0f, Frame.Height, Frame.Height);
		}

		void OnBackButtonClicked(object sender, EventArgs e) {
			BackButtonClicked(sender, e);
		}
	}
}
