using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace Magazine.Core
{
    public class DataBase
    {
        // Строка подключения к SQLite
        private readonly string _connectionString;

        // SQL‑запросы, оформленные в виде констант
        public const string CreateTableQuery =
            @"CREATE TABLE IF NOT EXISTS Products (
                ID TEXT PRIMARY KEY,
                Name TEXT NOT NULL,
                Definition TEXT,
                Price REAL,
                Image TEXT
            );";

        public const string CreateIndexQuery =
            @"CREATE INDEX IF NOT EXISTS IX_Products_Id ON Products(ID);";

        public const string SelectQuery =
            @"SELECT * FROM Products;";

        public const string InsertQuery =
            @"INSERT INTO Products (ID, Name, Definition, Price, Image)
              VALUES (@ID, @Name, @Definition, @Price, @Image);";

        public const string UpdateQuery =
            @"UPDATE Products 
              SET Name = @Name, Definition = @Definition, Price = @Price, Image = @Image 
              WHERE ID = @ID;";

        public const string DeleteQuery =
            @"DELETE FROM Products WHERE ID = @ID;";

        public DataBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqliteConnection GetConnection() => new SqliteConnection(_connectionString);

        /// <summary>
        /// Выполнение запросов, не возвращающих результат (CREATE, INSERT, UPDATE, DELETE).
        /// </summary>
        public void ExecuteNonQuery(string query, Action<SqliteCommand> parameterSetup = null)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    parameterSetup?.Invoke(command);
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Выполнение SELECT запроса, возвращающего DataTable.
        /// </summary>
        public DataTable ExecuteQuery(string query, Action<SqliteCommand> parameterSetup = null)
        {
            var dt = new DataTable();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    parameterSetup?.Invoke(command);
                    using (var reader = command.ExecuteReader())
                    {
                        dt.Load(reader);
                    }
                }
            }
            return dt;
        }
    }
}

