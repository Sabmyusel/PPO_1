using System.Data;
using Microsoft.Data.Sqlite;

namespace Magazine.Core
{
    public class DataBase
    {
        private readonly string _connectionString;

        public const string CreateTable =
            @"CREATE TABLE IF NOT EXISTS Products (
                ID TEXT PRIMARY KEY,
                Name TEXT NOT NULL,
                Definition TEXT,
                Price REAL,
                Image TEXT
            );";

        public const string CreateIndex =
            @"CREATE INDEX IF NOT EXISTS I_Products_Id ON Products(ID);";

        public const string SelectQuery =
            @"SELECT * FROM Products;";

        public const string InsertQuery =
            @"INSERT INTO Products (ID, Name, Definition, Price, Image)
              VALUES ('{0}', '{1}', '{2}', '{3}', '{4}');";

        public const string UpdateQuery =
            @"UPDATE Products 
              SET Name = '{1}', Definition = '{2}', Price = '{3}', Image = '{4}' 
              WHERE ID = '{0}';";

        public const string SearchQuery =
            @"SELECT * FROM Products WHERE ID = '{0}';";

        public const string DeleteQuery =
            @"DELETE FROM Products WHERE ID = '{0}';";

        public DataBase(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Выполнение запроса, не возвращающего результат (CREATE, INSERT, UPDATE, DELETE).
        /// </summary>
        public void ExecuteNonRet(string query)
        {
            SqliteConnection connection = new SqliteConnection(_connectionString);
            try
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                try
                {
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                }
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
        }

        /// <summary>
        /// Выполнение запроса, возвращающего DataTable.
        /// </summary>
        public DataTable ExecuteRet(string query)
        {
            DataTable dt = new DataTable();
            SqliteConnection connection = new SqliteConnection(_connectionString);
            try
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                try
                {
                    command.CommandText = query;
                    SqliteDataReader reader = command.ExecuteReader();
                    try
                    {
                        dt.Load(reader);
                    }
                    finally
                    {
                        reader.Close();
                        reader.Dispose();
                    }
                }
                finally
                {
                    command.Dispose();
                }
            }
            finally
            {
                connection.Close();
                connection.Dispose();
            }
            return dt;
        }
    }
}
