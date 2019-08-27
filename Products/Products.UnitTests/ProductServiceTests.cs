using NUnit.Framework;
using Products.Models;
using Products.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Products.Services;

namespace Products.UnitTests
{
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepository;
        private ProductService _productService;
        private ModelStateWrapper _modelStateWrapper;

        [SetUp]
        public void Setup()
        {
            _productRepository = new Mock<IProductRepository>();
            _modelStateWrapper = new ModelStateWrapper(new ModelStateDictionary());
            _productService = new ProductService(_productRepository.Object, null);
        }

        [Test]
        public void UpdateExistingProduct_IsValid()
        {
            const string productId = "existingId";
            _productRepository.Setup(p => p.Get(productId)).Returns(new Product());
            _productService.ValidateIsUpdatingExistingProduct(productId, _modelStateWrapper);
            Assert.IsTrue(_modelStateWrapper.IsValid);
        }

        [Test]
        public void UpdateNewProduct_IsInvalid()
        {
            _productService.ValidateIsUpdatingExistingProduct("newId", _modelStateWrapper);
            Assert.IsFalse(_modelStateWrapper.IsValid);
        }
    }
}