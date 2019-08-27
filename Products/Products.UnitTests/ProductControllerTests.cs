using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Products.Controllers;
using Products.Models;
using Products.Services;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;

namespace Products.UnitTests
{
    public class ProductControllerTests
    {
        private Mock<IProductService> _productService;
        private ProductsController _productController;

        [SetUp]
        public void Setup()
        {
            _productService = new Mock<IProductService>();
            _productController = new ProductsController(_productService.Object);
        }

        [Test]
        public void GetNonExistentProductId_ReturnsNotFound()
        {
            var actionResult = _productController.Get("non existent productId");

            Assert.IsInstanceOf<NotFoundResult>(actionResult.Result);
        }

        [Test]
        public void GetExistentProductId_ReturnsExpectedProduct()
        {
            const string productId = "productId";
            var setupProduct = new Product { Id = productId };

            _productService.Setup(p => p.Get(productId)).Returns(setupProduct);

            var actionResult = _productController.Get(productId);
            
            Assert.AreEqual(setupProduct, actionResult.Value);
        }

        [Test]
        public void PostProduct_ReturnsCreatedAtAction()
        {
            var product = new ProductRequest { Brand = "LG", Model = "OLED TV", Description = "TV from LG"};
            _productService.Setup(p => p.Create(product)).Returns(new Product());

            var actionResult = _productController.Post(product);

            Assert.IsInstanceOf<CreatedAtActionResult>(actionResult.Result);
        }
    }
}