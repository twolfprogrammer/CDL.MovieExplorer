using System;
using CoreGraphics;
using MovieExplorer.Core.Values;
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
			BackgroundColor = Colors.GrayDark.AsUIColor();
			titleLabel = new UILabel {
				Text = Core.Values.Views.General.HeaderTitle,
				TextColor = Colors.GoldDark.AsUIColor(),
				TextAlignment = UITextAlignment.Center,
				Font = UIFont.FromName(Core.Values.Views.General.HelveticaFontBold, 18f),
			};
			AddSubview(titleLabel);

			backButton = new UIButton {
				BackgroundColor = UIColor.Clear,
				Font = UIFont.FromName(IconFont.Name, 25f),
			};
			backButton.SetTitle(IconFont.IconClose, UIControlState.Normal);
			backButton.SetTitleColor(Colors.White.AsUIColor(), UIControlState.Normal);
			backButton.SetTitleColor(Colors.Gray.AsUIColor(), UIControlState.Highlighted);
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
