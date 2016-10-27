using System;
namespace MovieExplorer.Core {
	public class FavoriteMovie : SQLiteBaseItem {
		public int MovieId { get; set; }
		public string Title { get; set; }
		public FavoriteMovie() { }
		public FavoriteMovie(int movieId, string title) {
			MovieId = movieId;
			Title = title;
		}
	}
}
