using System;
using UIKit;

namespace MovieExplorer.iOS {
	public abstract class BasePageViewController : UIViewController {

		BasePageView basePageView {
			get {
				return ((BasePageView)View);
			}
		}

		public BasePageViewController() {
		}

		public override void ViewWillAppear(bool animated) {
			base.ViewDidAppear(animated);
			if (NavigationController != null) {
				basePageView.ShowBackButton = NavigationController.ViewControllers.Length > 1;
				basePageView.BackButtonClicked += GoBack;
			}
		}

		public override void ViewWillDisappear(bool animated) {
			base.ViewWillDisappear(animated);
			basePageView.BackButtonClicked -= GoBack;
		}

		protected void GoBack(object sender, EventArgs e) {
			if (NavigationController != null && NavigationController.ViewControllers.Length > 1) {
				NavigationController.PopViewController(animated: true);
			}
		}
	}
}
