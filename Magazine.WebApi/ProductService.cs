using Magazine.Core.Models;
using Magazine.Core.Services;
using System.Text.Json;

namespace Magazine.WebApi
{
    public class ProductService : IProductService
    {
        // Сохраняем конфигурацию во внутренней переменной для дальнейшего использования
        private readonly IConfiguration _configuration;
        // Путь до файла базы данных
        private readonly string _dbFilePath;

        private readonly Dictionary<Guid, Product> _products = new();

        /// <summary>
        /// Читает данные из файла и десериализует их в словарь (in-memory копия базы данных).
        /// </summary>
        /// <returns>Словарь продуктов. Ключ – идентификатор (Guid), значение – объект Product.</returns>
        private Dictionary<Guid, Product> InitFromFile()
        {
            if (!File.Exists(_dbFilePath))
            {
                // Если файла нет возвращаем пустой 
                return new Dictionary<Guid, Product>();
            }
            string json = File.ReadAllText(_dbFilePath);
            var products = JsonSerializer.Deserialize<Dictionary<Guid, Product>>(json);
            return products ?? new Dictionary<Guid, Product>();
        }

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;

            _dbFilePath = _configuration["DataBaseFilePath"];
            
            _products = InitFromFile();
        }
        public Product Add(Product product)
        {
            product.Id = Guid.NewGuid();
            _products[product.Id] = product;
            WriteToFile();
            return product;
        }
        public Product? Remove(Guid id)
        {
            if (_products.TryGetValue(id, out var product))
            {
                _products.Remove(id);
                WriteToFile();
                return product;
            }
            return null;
        }
        public Product? Edit(Product product)
        {
            if (_products.ContainsKey(product.Id))
            {
                _products[product.Id] = product;
                WriteToFile();
                return product;
            }
            return null;
        }

        public Product? Search(Guid id)
        {
            _products.TryGetValue(id, out var product);
            return product;
        }

        // Мьютекс для записи в файл
        private readonly Mutex _mutex = new Mutex();

        private void WriteToFile()
        {
            _mutex.WaitOne();
            var json = JsonSerializer.Serialize(_products, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dbFilePath, json);
            _mutex.ReleaseMutex();
        }

    }
}
