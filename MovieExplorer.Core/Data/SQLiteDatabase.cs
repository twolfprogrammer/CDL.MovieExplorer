using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace MovieExplorer.Core {
	internal class SQLiteDatabase {
		static object locker = new object();
		ISQLite SQLite;
		SQLiteConnection Database;
		string DatabaseName;

		public SQLiteDatabase(string databaseName, ISQLite sqlite) {
			DatabaseName = databaseName;
			SQLite = sqlite;
			Database = SQLite.GetConnection(DatabaseName);
		}

		/// <summary>
		/// Creates a table in the database named by the generic type T.
		/// If a table exists, nothing will happen.
		/// </summary>
		/// <typeparam name="T">The name of the table and items type</typeparam>
		public void CreateTable<T>() {
			lock (locker) {
				Database.CreateTable<T>();
			}
		}

		/// <summary>
		/// Saves an item of type T to the table of type T
		/// </summary>
		/// <returns>The item's id</returns>
		/// <param name="item">A generic item of type T</param>
		/// <typeparam name="T">The name of the table and items type</typeparam>
		public int SaveItem<T>(T item) {
			lock (locker) {
				var id = ((SQLiteBaseItem)(object)item).ID;
				if (id != 0) {
					Database.Update(item);
					return id;
				}
				else {
					return Database.Insert(item);
				}
			}
		}

		/// <summary>
		/// Executes a nonreturnable query. 
		/// </summary>
		/// <param name="query">The query string</param>
		/// <param name="args">The ordered arguments for the query denoted by ? in the query string.</param>
		public void ExecuteQuery(string query, object[] args) {
			lock (locker) {
				Database.Execute(query, args);
			}
		}

		/// <summary>
		/// Finds an item of type T from the table of T based on a query
		/// </summary>
		/// <returns>An item of type T</returns>
		/// <param name="query">A select query</param>
		/// <param name="args">The ordered arguments for the query denoted by ? in the query string.</param>
		/// <typeparam name="T">The name of the table and items type</typeparam>
		public T FindWithQuery<T>(string query, object[] args) where T : new() {
			lock (locker) {
				return Database.FindWithQuery<T>(query, args);
			}
		}

		/// <summary>
		/// Gets all the items in the table named T of the type T 
		/// </summary>
		/// <returns>An enumerable list of type T</returns>
		/// <typeparam name="T">The name of the table and items type</typeparam>
		public IEnumerable<T> GetItems<T>() where T : new() {
			lock (locker) {
				return (from i in Database.Table<T>() select i).ToList();
			}
		}

		/// <summary>
		/// Deletes the item with the id from the table of T
		/// </summary>
		/// <param name="id">The id of the item</param>
		/// <typeparam name="T">The name of the table and items type</typeparam>
		public void DeleteItem<T>(int id) {
			lock (locker) {
				Database.Delete<T>(id);
			}
		}

		/// <summary>
		/// Deletes all the items in the table of T
		/// </summary>
		/// <typeparam name="T">The name of the table and items type</typeparam>
		public void DeleteAll<T>() {
			lock (locker) {
				Database.DeleteAll<T>();
			}
		}
	}
}
