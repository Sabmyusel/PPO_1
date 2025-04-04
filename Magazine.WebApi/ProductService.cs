using Magazine.Core.Models;
using Magazine.Core.Services;
using Magazine.Core;
using System.Data;

namespace Magazine.WebApi
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly string _dbFilePath;
        private readonly DataBase _database;
        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
            _dbFilePath = _configuration["DataBaseFilePath"];

            string connectionString = $"Data Source={_dbFilePath};Pooling=False";

            _database = new DataBase(connectionString);

            // Создаем таблицу и индекс 
            _database.ExecuteNonRet(DataBase.CreateTable);
            _database.ExecuteNonRet(DataBase.CreateIndex);
        }

        public Product Add(Product product)
        {
            product.Id = Guid.NewGuid();

            string query = string.Format(DataBase.InsertQuery, product.Id, product.Name, product.Definition, product.Price, product.Image);
            _database.ExecuteNonRet(query);
            return product;
        }

        public Product? Remove(Guid id)
        {
            Product product = Search(id);

            string query = string.Format(DataBase.DeleteQuery, id);
            _database.ExecuteNonRet(query);
            return product;
        }

        public Product? Edit(Product product)
        {
            string query = string.Format(DataBase.UpdateQuery, product.Id, product.Name, product.Definition, product.Price, product.Image);
            _database.ExecuteNonRet(query);
            return Search(product.Id);
        }

        public Product? Search(Guid id)
        {
            string query = string.Format(DataBase.SearchQuery, id);
            DataTable dt = _database.ExecuteRet(query);
            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];
            return new Product
            {
                Id = Guid.Parse(row["ID"].ToString()),
                Name = row["Name"].ToString(),
                Definition = row["Definition"].ToString(),
                Price = (float)Convert.ToDecimal(row["Price"]),
                Image = row["Image"].ToString()
            };
        }
    }
}
