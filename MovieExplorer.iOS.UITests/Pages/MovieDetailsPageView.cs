using System;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace MovieExplorer.iOS.UITests {
	[TestFixture]
	public class MovieDetailsPageView {
		IApp app;

		[SetUp]
		public void Setup() {
			app = ConfigureApp
			  .iOS
			  .AppBundle("/Users/thomas/Development/Wolf/MovieExplorer/MovieExplorer.iOS/bin/iPhoneSimulator/Debug/MovieExplorer.iOS.app")
			  .StartApp();
		}
		[Test]
		public void SaveToFavoriteButtonLabel_Clicked_ChangesToRemoveFromFavorites() {
			// Arrange

			// Act
			app.Tap(x => x.Class("UIImageView").Index(2));
			app.Screenshot("Tapped on view with class: UIImageView");
			app.Tap(x => x.Text("Save to Favorites"));
			app.Screenshot("Tapped on view with class: UIButtonLabel marked: Save to Favorites");

			// Query
			var result = app.Query(c => c.Class("UIButtonLabel").Text("Remove from Favorites"));

			// Cleanup
			app.Tap(x => x.Text("Remove from Favorites"));
			app.Screenshot("Tapped on view with class: UIButtonLabel marked: Remove from Favorites");

			// Assert
			Assert.IsTrue(result.Length > 0);
		}
	}
}
