using System;
namespace MovieExplorer.Core.Values.Views {
	public static class MovieDetailsPageView {
		public const string DateFormat = "MM/dd/yyyy";
		public static string GetReleaseDate(DateTime releaseDate) => $"Release Date: {releaseDate.Date.ToString(DateFormat)}";
		public static string GetRatingIcons(double rating) {
			string ratingString = "";
			for (int i = 0; i < 5; i++) {
				ratingString += i < (rating / 2) ? Core.Values.IconFont.IconStarFull
									: Core.Values.IconFont.IconStarEmpty;
			}
			return ratingString;
		}
		public static string GetVoteCount(int count) => $"(from {count} votes)";
		public const string PlayVideo = "Play Video";
		public const string AddToFavorites = "Add to Favorites";
		public const string RemoveFromFavorites = "Remove from Favorites";
	}
}
