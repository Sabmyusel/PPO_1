using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Magazine.WebApi.Controllers;
using Magazine.Core.Services;
using Magazine.Core.Models;
using System.Text.Encodings.Web;
using System.Text.Json;
using Magazine.WebApi;
using Moq;
using Microsoft.Extensions.Configuration;
namespace Magazine.Test
{
    [TestFixture]
    public class TestsProductController
    {
        private ProductController _controller;
        private IProductService _service;
        [SetUp]
        public void Setup()
        {
            // mock IConfiguration
            var mockConfig = new Mock<IConfiguration>();
            // значение для ключа DataBaseFilePath
            mockConfig.Setup(c => c["DataBaseFilePath"]).Returns("test_database.txt");
            _service = new ProductService(mockConfig.Object);

            _controller = new ProductController(_service);
        }

        [Test]
        public void Add_Product_Should_Return_Product()
        {
            string name = "Phone";
            string description = "Smartphone";
            float price = 500;
            string image = "ImageLink";

            var result = _controller.Add(name, price, description, image);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(result.Price, Is.EqualTo(price));
            Assert.That(result.Definition, Is.EqualTo(description));
            Assert.That(result.Image, Is.EqualTo(image));
        }

        [Test]
        public void Search_Product_Should_Return_Product()
        {
            string name = "Notepad";
            string description = "a note and  apad";
            float price = 1100;
            string image = "ImageLink";

            var added_prod = _controller.Add(name, price, description, image);
            var result = _controller.Search(added_prod.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        public void Remove_Should_Remove_From_DB_And_Return_Product()
        {
            string name = "Notepad";
            string description = "a note and  apad";
            float price = 1100;
            string image = "ImageLink";

            var added_prod = _controller.Add(name, price, description, image);
            var result = _controller.Remove(added_prod.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(name));
            Assert.That(_controller.Search(result.Id), Is.Null);
        }

        [Test]
        public void Edit_Should_Change_Params_Not_ID()
        {
            string name = "Notepad";
            string description = "a note and  apad";
            float price = 1100;
            string image = "ImageLink";
            var edit_prod = new Product {Name = "Not notepad", Price = 10, Definition = "NO", Image = "img" };

            var added_prod = _controller.Add(name, price, description, image);
            var result = _controller.Edit(added_prod.Id, edit_prod.Name, edit_prod.Price, edit_prod.Definition, edit_prod.Image);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(added_prod.Id));
            Assert.That(result.Name, Is.EqualTo(edit_prod.Name));
            Assert.That(result.Price, Is.EqualTo(edit_prod.Price));
            Assert.That(result.Definition, Is.EqualTo(edit_prod.Definition));
            Assert.That(result.Image, Is.EqualTo(edit_prod.Image));
        }
    }
}

