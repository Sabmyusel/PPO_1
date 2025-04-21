using System;
using System.Linq;
using Magazine.Core.Data;
using Magazine.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Magazine.Core.Services
{
    public class DataBaseProductService : IProductService
    {
        private readonly ApplicationContext _context;

        public DataBaseProductService(ApplicationContext context)
        {
            _context = context;
        }

        public Product Add(Product product)
        {
            product.Id = Guid.NewGuid();
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product Remove(Guid id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return product;
        }

        public Product Edit(Product product)
        {
            var existing = _context.Products.Find(product.Id);
            if (existing == null)
                return null;

            existing.Name = product.Name;
            existing.Definition = product.Definition;
            existing.Price = product.Price;
            existing.Image = product.Image;

            _context.Products.Update(existing);
            _context.SaveChanges();
            return existing;
        }

        public Product Search(Guid id)
        {
            return _context.Products.Find(id);
        }
    }
}
