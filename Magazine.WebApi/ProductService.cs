using Magazine.Core.Models;
using Magazine.Core.Services;

namespace Magazine.WebApi
{
    public class ProductService : IProductService
    {
        private readonly List<Product> _products = new();
        public Product Add(Product product)
        {
            product.Id = Guid.NewGuid();
            _products.Add(product);
            return product;
        }

        public Product? Remove(Guid id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
            return product;
        }

        public Product? Edit(Product product)
        {
            var foundProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (foundProduct != null)
            {
                foundProduct.Name = product.Name;
                foundProduct.Definition = product.Definition;
                foundProduct.Price = product.Price;
                foundProduct.Image = product.Image;
            }
            return foundProduct;
        }

        public Product? Search(string name)
        {
            return _products.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
