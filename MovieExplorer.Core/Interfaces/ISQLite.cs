using System;
using SQLite;

namespace MovieExplorer.Core {
	public interface ISQLite {
		/// <summary>
		/// Gets the database connection. It will create the database if it does not exist.
		/// </summary>
		SQLiteConnection GetConnection(string databaseName);
	}
}
