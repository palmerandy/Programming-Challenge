using System;
using Products.Models;
using System.Collections.Generic;
using System.Linq;

namespace Products.Repository
{
    public interface IProductRepository
    {
        List<Product> Get(string brand, string model, string description);
        Product Get(string id);
        Product Create(Product product);
        Product Update(Product product);
        void Delete(string id);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly Dictionary<string, Product> _products;

        public ProductRepository()
        {
            _products = new Dictionary<string, Product>();
        }

        public List<Product> Get(string brand, string model, string description)
        {
            return _products.Values.Where(p=>
                    Predicate(brand, p.Brand) &&
                    Predicate(model, p.Model) &&
                    Predicate(description, p.Description)
            ).ToList();
        }

        private static bool Predicate(string filter, string fieldName)
        {
            return filter == null || fieldName.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
        }

        public Product Get(string id)
        {
            return _products.ContainsKey(id) ? _products[id] : null;
        }

        public Product Create(Product product)
        {
            _products[product.Id] = product;
            return product;
        }
        
        public Product Update(Product product)
        {
            if (!_products.ContainsKey(product.Id))
            {
                return null;
            }
            _products[product.Id] = product;
            return product;
        }

        public void Delete(string id)
        {
            if (_products.ContainsKey(id))
            {
                _products.Remove(id);
            }
        }
    }
}