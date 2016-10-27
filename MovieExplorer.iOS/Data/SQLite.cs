using System;
using System.IO;
using MovieExplorer.Core;
using SQLite;

namespace MovieExplorer.iOS {
	public class SQLite : ISQLite {
		string GetPath(string databaseName) {
			if (string.IsNullOrWhiteSpace(databaseName)) {
				throw new Exception(Core.Values.Errors.InvalidDatabaseFilename);
			}
			var sqliteFilename = $"{databaseName}.{Core.Values.SQLite.FilenameExtension}";
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var libraryPath = Path.Combine(documentsPath, "..", "Library");
			var path = Path.Combine(libraryPath, sqliteFilename);
			return path;
		}

		public SQLiteConnection GetConnection(string databaseName) {
			return new SQLiteConnection(GetPath(databaseName));
		}
	}
}
