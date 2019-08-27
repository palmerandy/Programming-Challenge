using Products.Models;
using System.Collections.Generic;
using Products.Repository;

namespace Products.Services
{
    public interface IProductService
    {
        List<Product> Get(string brand, string model, string description);
        Product Get(string id);
        Product Create(ProductRequest product);
        Product Update(string id, ProductRequest productRequest, IValidationDictionary modelStateWrapper);
        void Delete(string id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IProductFactory _factory;

        public ProductService(IProductRepository repository, IProductFactory factory)
        {
            _repository = repository;
            _factory = factory;
        }
        public List<Product> Get(string brand, string model, string description)
        {
            return _repository.Get(brand, model, description);
        }

        public Product Get(string id)
        {
            return _repository.Get(id);
        }

        public Product Create(ProductRequest productRequest)
        {
            var product = _factory.BuildWithNewId(productRequest);
            return _repository.Create(product);
        }
        
        public Product Update(string productId, ProductRequest productRequest, IValidationDictionary validationDictionary)
        {
            ValidateIsUpdatingExistingProduct(productId, validationDictionary);
            if (!validationDictionary.IsValid)
            {
                return null;
            }
            var product = _factory.BuildWithExistingId(productId, productRequest);
            return _repository.Update(product);
        }

        public void Delete(string id)
        {
            _repository.Delete(id);
        }

        public void ValidateIsUpdatingExistingProduct(string productId, IValidationDictionary validationDictionary)
        {
            if (_repository.Get(productId) == null)
            {
                validationDictionary.AddModelError(nameof(productId), "ProductId does not exist. Please use Post for creating a Product and Put for updating a product.");
            }
        }
    }
}