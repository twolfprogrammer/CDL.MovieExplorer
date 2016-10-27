using System;
using SQLite;

namespace MovieExplorer.Core {
	public class SQLiteBaseItem {
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
	}
}
