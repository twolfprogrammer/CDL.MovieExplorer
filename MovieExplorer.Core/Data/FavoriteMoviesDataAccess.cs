using System;
namespace MovieExplorer.Core {
	public class FavoriteMoviesDataAccess {

		SQLiteDatabase database;

		static FavoriteMoviesDataAccess instance;
		internal static FavoriteMoviesDataAccess Instance {
			get {
				if (instance == null) {
					throw new Exception(Values.Errors.FavoriteMovieServiceNotInitialized);
				}
				return instance;
			}
		}

		private FavoriteMoviesDataAccess() { }

		internal static void Initialize(ISQLite sqlite) {
			instance = new FavoriteMoviesDataAccess();
			instance.database = new SQLiteDatabase(Values.SQLite.DatabaseName, sqlite);
			instance.database.CreateTable<FavoriteMovie>();
		}

		public bool SaveMovie(int movieId, string title) {
			if (!ContainsMovie(movieId)) {
				database.SaveItem<FavoriteMovie>(new FavoriteMovie(movieId, title));
			}
			return ContainsMovie(movieId);
		}

		public bool RemoveMovie(int movieId) {
			var movie = FindMovie(movieId);
			if (movie != null) {
				database.DeleteItem<FavoriteMovie>(movie.ID);
			}
			return !ContainsMovie(movieId);
		}

		public bool ContainsMovie(int movieId) {
			var movie = FindMovie(movieId);
			return movie != null && movie.MovieId == movieId;
		}

		FavoriteMovie FindMovie(int movieId) {
			return database.FindWithQuery<FavoriteMovie>(
				Values.SQLite.SelectFavoriteMovieByIdQuery,
				new object[] { movieId });
		}
	}
}
