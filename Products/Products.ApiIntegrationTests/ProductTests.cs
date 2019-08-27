using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Products.ApiIntegrationTests.ApiResponses;

namespace Products.ApiIntegrationTests
{
    public class ProductTests : BaseTest
    {
        private const string ProductApi = "products";

        [Test]
        public async Task GetNonExistentProductId_ReturnsNotFound()
        {
            var nonExistentProductId = Guid.NewGuid().ToString();

            var response = await Get(nonExistentProductId);

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task PostIncompleteProduct_ReturnsBadRequest()
        {
            var requestProduct = new RequestProduct();

            var (statusCode, _) = await SaveProduct(requestProduct, null, Post);

            Assert.AreEqual(HttpStatusCode.BadRequest, statusCode);
        }

        [Test]
        public async Task PostProduct_CanFindPostedProductByGetAll()
        {
            var requestProduct = new RequestProduct { Brand = "Apple", Model = "MacBook Pro", Description = "Laptop from Apple" };

            var (statusCode, product) = await SaveProduct(requestProduct, null, Post);
            AssertSuccessfulSave(requestProduct, statusCode, product);

            var response = await Get();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var products = await ReadContent<List<Product>>(response);

            var savedProduct = products.Single(p => p.Id == product.Id);
            Assert.IsNotNull(savedProduct);
        }

        
        [Test]
        public async Task PostProduct_ThenCanDeletePostedProduct()
        {
            var requestProduct = new RequestProduct { Brand = "Samsung", Model = "Fridge", Description = "Fridge from Samsung" };

            var (statusCode, product) = await SaveProduct(requestProduct, null, Post);
            AssertSuccessfulSave(requestProduct, statusCode, product);

            var response = await Get(product.Id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            response = await Client.DeleteAsync($"{ProductApi}/{product.Id}");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

            response = await Get(product.Id);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Test]
        public async Task PostThenPutProduct_UpdateSucceeds()
        {
            var requestProduct = new RequestProduct { Brand = "Dell", Model = "Xps", Description = "Laptop from Dell" };

            var (statusCode, product) = await SaveProduct(requestProduct, null, Post);
            AssertSuccessfulSave(requestProduct, statusCode, product);

            (statusCode, product) = await SaveProduct(requestProduct, product.Id, Put);
            Assert.AreEqual(HttpStatusCode.NoContent, statusCode);
            Assert.IsNull(product);
        }

        [Test]
        public async Task RemovalOfApi_ReturnsUnauthorized()
        {
            RemoveApiKeyAuthorization();
            var response = await Get();
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);


            AddApiKeyAuthorization();
            response = await Get();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        private async Task<(HttpStatusCode StatusCode, Product Product)> SaveProduct(RequestProduct requestProduct, string productId, Func<string, HttpContent, Task<HttpResponseMessage>> saveFunc)
        {
            var stringContent = BuildStringContent(requestProduct);
            var response = await saveFunc(productId, stringContent);
            var responseProduct = await ReadContent<Product>(response);
            return (response.StatusCode, responseProduct);
        }

        private static void AssertSuccessfulSave(RequestProduct requestProduct, HttpStatusCode statusCode, Product responseProduct)
        {
            Assert.AreEqual(HttpStatusCode.Created, statusCode);
            Assert.AreEqual(requestProduct.Brand, responseProduct.Brand);
            Assert.AreEqual(requestProduct.Model, responseProduct.Model);
            Assert.AreEqual(requestProduct.Description, responseProduct.Description);
        }
        
        private async Task<HttpResponseMessage> Put(string productId, HttpContent stringContent)
        {
            return await Client.PutAsync($"{ProductApi}/{productId}", stringContent);
        }

        private async Task<HttpResponseMessage> Post(string productId, HttpContent stringContent)
        {
            return await Client.PostAsync(ProductApi, stringContent);
        }

        private async Task<HttpResponseMessage> Get(string id)
        {
            return await Client.GetAsync($"{ProductApi}/{id}");
        }

        private async Task<HttpResponseMessage> Get()
        {
            return await Client.GetAsync(ProductApi);
        }
    }
}