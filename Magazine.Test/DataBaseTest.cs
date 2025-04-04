using System.Data;
using Magazine.Core;


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
            _tempFile = "testFile.db";
            if (File.Exists(_tempFile))
            {
                File.Delete(_tempFile);
            }
            _connectionString = $"Data Source={_tempFile};Pooling=False";
            _database = new DataBase(_connectionString);

            // Создаем таблицу и индекс
            _database.ExecuteNonRet(DataBase.CreateTable);
            _database.ExecuteNonRet(DataBase.CreateIndex);
        }

        [Test]
        public void Test_CreateTable_And_Index()
        {
            DataTable dt = _database.ExecuteRet(DataBase.SelectQuery);
            Assert.That(dt.Rows.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_Insert_And_Select_Product()
        {
            Guid id = Guid.NewGuid();
            string name = "Product",
                definition = "Def",
                image = "image.png";
            float price = 1.5f;
            string querry = string.Format(DataBase.InsertQuery, id, name, definition, price, image);
            
            _database.ExecuteNonRet(querry);

            DataTable dt = _database.ExecuteRet(DataBase.SelectQuery);
            Assert.That(dt.Rows.Count, Is.EqualTo(1));
            Assert.That(dt.Rows[0]["Name"], Is.EqualTo(name));
            Assert.That(dt.Rows[0]["ID"], Is.EqualTo(id.ToString()));
            Assert.That(dt.Rows[0]["Definition"], Is.EqualTo(definition));
            Assert.That(dt.Rows[0]["Image"], Is.EqualTo(image));
            Assert.That(dt.Rows[0]["Price"], Is.EqualTo(price.ToString()));
        }

        [Test]
        public void Test_Update_Product()
        {
            Guid id = Guid.NewGuid();
            string name = "Product",
                definition = "Def",
                image = "image.png";
            float price = 1.5f;
            // Вставляем продукт
            string querry = string.Format(DataBase.InsertQuery, id, name, definition, price, image);

            _database.ExecuteNonRet(querry);
            
            name = "New name";
            definition = "New def";
            image = "new image.png";
            price = 10.5f;
            // Обновляем продукт
            querry = string.Format(DataBase.UpdateQuery, id, name, definition, price, image);
            
            _database.ExecuteNonRet(querry);

            DataTable dt = _database.ExecuteRet(DataBase.SelectQuery);
            Assert.That(dt.Rows[0]["Name"], Is.EqualTo(name));
            Assert.That(dt.Rows[0]["ID"], Is.EqualTo(id.ToString()));
            Assert.That(dt.Rows[0]["Definition"], Is.EqualTo(definition));
            Assert.That(dt.Rows[0]["Image"], Is.EqualTo(image));
            Assert.That(dt.Rows[0]["Price"], Is.EqualTo(price.ToString()));
        }

        [Test]
        public void Test_Delete_Product()
        {
            Guid id = Guid.NewGuid();
            string name = "Product",
                definition = "Def",
                image = "image.png";
            float price = 1.5f;
            // Вставляем продукт
            string querry = string.Format(DataBase.InsertQuery, id, name, definition, price, image);

            _database.ExecuteNonRet(querry);

            querry = string.Format(DataBase.DeleteQuery, id);
            // Удаляем продукт
            _database.ExecuteNonRet(querry);

            DataTable dt = _database.ExecuteRet(DataBase.SelectQuery);
            Assert.That(dt.Rows.Count, Is.EqualTo(0));
        }
    }
}
