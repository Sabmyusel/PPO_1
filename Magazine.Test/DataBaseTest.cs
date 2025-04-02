using System;
using System.Data;
using System.IO;
using Magazine.Core;
using Microsoft.Data.Sqlite;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace Magazine.Test
{
    [TestFixture]
    public class DataBaseTests
    {
        private DataBase _database;
        private string _connectionString;
        private string _tempFile;

        [SetUp]
        public void Setup()
        {
            // Генерируем уникальное имя файла для тестовой базы
            _tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".db");
            _connectionString = $"Data Source={_tempFile}";
            _database = new DataBase(_connectionString);

            // Создаем таблицу и индекс
            _database.ExecuteNonQuery(DataBase.CreateTableQuery);
            _database.ExecuteNonQuery(DataBase.CreateIndexQuery);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_tempFile))
            {
                File.Delete(_tempFile);
            }
        }

        [Test]
        public void Test_CreateTable_And_Index()
        {
            // Попытаемся вставить продукт – если таблица создана, вставка пройдет успешно
            var id = Guid.NewGuid().ToString();
            _database.ExecuteNonQuery(DataBase.InsertQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", "Test Product");
                cmd.Parameters.AddWithValue("@Definition", "Test Definition");
                cmd.Parameters.AddWithValue("@Price", 10.5);
                cmd.Parameters.AddWithValue("@Image", "TestImage.png");
            });

            DataTable dt = _database.ExecuteQuery(DataBase.SelectQuery);
            Assert.That(dt.Rows.Count, Is.EqualTo(1));
        }

        [Test]
        public void Test_Insert_And_Select_Product()
        {
            var id = Guid.NewGuid().ToString();
            _database.ExecuteNonQuery(DataBase.InsertQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", "Product A");
                cmd.Parameters.AddWithValue("@Definition", "Definition A");
                cmd.Parameters.AddWithValue("@Price", 20.0);
                cmd.Parameters.AddWithValue("@Image", "ImageA.png");
            });

            DataTable dt = _database.ExecuteQuery(DataBase.SelectQuery);
            Assert.That(dt.Rows.Count, Is.EqualTo(1));
            Assert.That(dt.Rows[0]["Name"], Is.EqualTo("Product A"));
        }

        [Test]
        public void Test_Update_Product()
        {
            var id = Guid.NewGuid().ToString();
            // Вставляем продукт
            _database.ExecuteNonQuery(DataBase.InsertQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", "Old Name");
                cmd.Parameters.AddWithValue("@Definition", "Old Definition");
                cmd.Parameters.AddWithValue("@Price", 30.0);
                cmd.Parameters.AddWithValue("@Image", "OldImage.png");
            });

            // Обновляем продукт
            _database.ExecuteNonQuery(DataBase.UpdateQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", "New Name");
                cmd.Parameters.AddWithValue("@Definition", "New Definition");
                cmd.Parameters.AddWithValue("@Price", 40.0);
                cmd.Parameters.AddWithValue("@Image", "NewImage.png");
            });

            DataTable dt = _database.ExecuteQuery(DataBase.SelectQuery);
            Assert.That(dt.Rows[0]["Name"], Is.EqualTo("New Name"));
        }

        [Test]
        public void Test_Delete_Product()
        {
            var id = Guid.NewGuid().ToString();
            // Вставляем продукт
            _database.ExecuteNonQuery(DataBase.InsertQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", "Product To Delete");
                cmd.Parameters.AddWithValue("@Definition", "Definition");
                cmd.Parameters.AddWithValue("@Price", 50.0);
                cmd.Parameters.AddWithValue("@Image", "Image.png");
            });

            // Удаляем продукт
            _database.ExecuteNonQuery(DataBase.DeleteQuery, cmd =>
            {
                cmd.Parameters.AddWithValue("@ID", id);
            });

            DataTable dt = _database.ExecuteQuery(DataBase.SelectQuery);
            Assert.That(dt.Rows.Count, Is.EqualTo(0));
        }
    }
}
