using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Magazine.Core.Services;
using Magazine.Core.Models;
using Magazine.WebApi;
using Microsoft.Extensions.Configuration;
using Moq;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
namespace Magazine.Test
{
    [TestFixture]
    public class TestsProductService
    {
        private ProductService _productService;

        [SetUp]
        public void Setup()
        {
            var conf = new Dictionary<string, string>
            {
                { "DataBaseFilePath", "DB.txt" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(conf)
                .Build();

            _productService = new ProductService(configuration);
        }

        [Test]
        public void Add_Product_Should_Add_To_List()
        {
            var product = new Product { Name = "Test", Definition = "Desc", Price = 100 };
            var result = _productService.Add(product);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(result.Price, Is.EqualTo(product.Price));
            Assert.That(result.Definition, Is.EqualTo(product.Definition));
            Assert.That(result.Image, Is.EqualTo(product.Image));
        }

        [Test]
        public void Search_Product_Should_Return_Product()
        {
            var product = new Product
            {
                Name = "Notepad",
                Definition = "a note and  apad",
                Price = 1100,
                Image = "ImageLink"
            };

            var added_prod = _productService.Add(product);
            var result = _productService.Search(added_prod.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(product.Name));
        }

        [Test]
        public void Remove_Should_Remove_From_DB_And_Return_Product()
        {
            var product = new Product
            {
                Name = "Notepad",
                Definition = "a note and  apad",
                Price = 1100,
                Image = "ImageLink"
            };

            var added_prod = _productService.Add(product);
            var result = _productService.Remove(added_prod.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(product.Name));
            Assert.That(_productService.Search(result.Id), Is.Null);
        }

        [Test]
        public void Edit_Should_Change_Params_Not_ID()
        {
            var product = new Product
            {
                Name = "Notepad",
                Definition = "a note and  apad",
                Price = 1100,
                Image = "ImageLink"
            };
            var edit_prod = new Product 
            { 
                Name = "Not notepad", 
                Price = 10, 
                Definition = "NO", 
                Image = "img"
            };

            var added_prod = _productService.Add(product);
            edit_prod.Id = added_prod.Id;
            var result = _productService.Edit(edit_prod);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(added_prod.Id));
            Assert.That(result.Name, Is.EqualTo(edit_prod.Name));
            Assert.That(result.Price, Is.EqualTo(edit_prod.Price));
            Assert.That(result.Definition, Is.EqualTo(edit_prod.Definition));
            Assert.That(result.Image, Is.EqualTo(edit_prod.Image));
        }
    }
}
