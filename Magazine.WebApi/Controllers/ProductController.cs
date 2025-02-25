using System.Xml.Linq;
using Magazine.Core.Models;
using Magazine.Core.Services;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Magazine.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Добавить новый продукт
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="description">Описание</param>
        /// <param name="price">Цена</param>
        /// <param name="image">Картинка</param>
        /// <returns>Добавленный продукт</returns>
        [HttpPost("{name},{price}")]
        public Product Add(string name, float price, string description = "desc", string image = "img")
        {
            if (string.IsNullOrEmpty(description) || price < 0)
            {
                return null;
            }
            var product = new Product()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Definition = description,
                Price = price,
                Image = image
            };
            _productService.Add(product);
            return product;
        }

        /// <summary>
        /// Удалить продукт по ID
        /// </summary>
        /// <param name="id">ID продукта</param>
        /// <returns>Удаленный продукт</returns>
        [HttpDelete("{id}")]
        public Product Remove(Guid id)
        {
            var removedProduct = _productService.Remove(id);
            if (removedProduct == null)
            {
                return null;
            }

            return removedProduct;
        }

        /// <summary>
        /// Изменить данные продукта
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Название</param>
        /// <param name="description">Описание</param>
        /// <param name="price">Цена</param>
        /// <param name="image">Картинка</param>
        /// <returns>Обновленный продукт</returns>
        [HttpPut("{id},{name},{price}")]
        public Product? Edit(Guid id, string name, float price, string description = "desc", string image = "img")
        {
            var product = new Product()
            {
                Id = id,
                Name = name,
                Definition = description,
                Price = price,
                Image = image
            };
            var updatedProduct = _productService.Edit(product);
            if (updatedProduct == null)
            {
                return null;
            }

            return updatedProduct;
        }

        /// <summary>
        /// Найти продукт по ID
        /// </summary>
        /// <param name="id">ID продукта</param>
        /// <returns>Найденный продукт</returns>
        [HttpGet("{id}")]
        public Product? Search(Guid id)
        {
            var product = _productService.Search(id);
            if (product == null)
            {
                return null;
            }

            return product;
        }
    }
}
