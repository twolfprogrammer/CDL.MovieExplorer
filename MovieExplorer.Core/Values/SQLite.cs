using System;
namespace MovieExplorer.Core.Values {
	public static class SQLite {
		public const string DatabaseName = "database";
		public const string VacuumQuery = "VACUUM";
		public const string FilenameExtension = "db3";

		// Favorite Movies
		public static readonly string SelectFavoriteMovieByIdQuery = 
			$"SELECT * FROM {nameof(FavoriteMovie)} WHERE {nameof(FavoriteMovie.MovieId)} = ?";

		// Request Cache
		public static readonly string DeleteCachedRequestLessThanDateTimeQuery =
			$"DELETE FROM {nameof(CachedRequest)} WHERE {nameof(DateTime)} < ?";
		public static readonly string SelectCachedRequestByUrlQuery =
			$"SELECT * FROM {nameof(CachedRequest)} WHERE {nameof(CachedRequest.Url)} = ?";
	}
}
