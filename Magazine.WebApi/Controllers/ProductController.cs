using Magazine.Core.Models;
using Magazine.Core.Services;
using Microsoft.AspNetCore.Mvc;

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
        /// <param name="product">Данные нового продукта</param>
        /// <returns>Добавленный продукт</returns>
        [HttpPost]
        public Product Add(Product product)
        {
            var addedProduct = _productService.Add(product);
            return addedProduct;
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
        /// <param name="product">Обновленные данные продукта</param>
        /// <returns>Обновленный продукт</returns>
        [HttpPut]
        public Product? Edit([FromBody] Product product)
        {
            if (product == null)
            {
                return null;
            }

            var updatedProduct = _productService.Edit(product);
            if (updatedProduct == null)
            {
                return null;
            }

            return updatedProduct;
        }

        /// <summary>
        /// Найти продукт по названию
        /// </summary>
        /// <param name="name">Название продукта</param>
        /// <returns>Найденный продукт</returns>
        [HttpGet("{name}")]
        public Product? Search(string name)
        {
            var product = _productService.Search(name);
            if (product == null)
            {
                return null;
            }

            return product;
        }
    }
}
