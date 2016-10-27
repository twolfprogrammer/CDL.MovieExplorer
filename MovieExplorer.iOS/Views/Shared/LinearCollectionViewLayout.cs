using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace MovieExplorer.iOS {
public class LinearCollectionViewLayout : UICollectionViewFlowLayout {
		public LinearCollectionViewLayout() {
			ItemSize = new CGSize(110, 180);
			ScrollDirection = UICollectionViewScrollDirection.Horizontal;
		}

		public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds) {
			return true;
		}

		public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath) {
			return base.LayoutAttributesForItem(indexPath);
		}

		public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect) {
			return base.LayoutAttributesForElementsInRect(rect);
		}
	}
}
