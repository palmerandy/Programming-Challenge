using System;
using Products.Models;

namespace Products.Services
{
    public interface IProductFactory
    {
        Product BuildWithNewId(ProductRequest request);
        Product BuildWithExistingId(string id, ProductRequest request);
    }

    public class ProductFactory : IProductFactory
    {
        public Product BuildWithNewId(ProductRequest request)
        {
            var id = Guid.NewGuid().ToString();
            return new Product {Id = id, Model = request.Model, Brand = request.Brand, Description = request.Description };
        }

        public Product BuildWithExistingId(string id, ProductRequest request)
        {
            return new Product { Id = id, Model = request.Model, Brand = request.Brand, Description = request.Description };
        }
    }
}
