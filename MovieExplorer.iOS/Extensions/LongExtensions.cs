using System;
using UIKit;

namespace MovieExplorer.iOS {
	public static class LongExtensions {
		public static UIColor AsUIColor(this long hexValue) {
			return UIColor.FromRGBA(
				(((float)((hexValue & 0xff0000) >> 16)) / 255.0f),
				(((float)((hexValue & 0xff00) >> 8)) / 255.0f),
				(((float)((hexValue & 0xff))) / 255.0f),
				(((float)((hexValue & 0xff000000) >> 24)) / 255.0f));
		}
	}
}
