using NUnit.Framework;
using Products.Models;
using Products.Repository;

namespace Products.UnitTests
{
    public class ProductRepositoryTests
    {
        private IProductRepository _productRepository;

        [SetUp]
        public void Setup()
        {
            _productRepository = new ProductRepository();

            _productRepository.Create(new Product { Id = "1", Brand = "Breville", Description = "Coffee Machine", Model = "Nespresso" });
            _productRepository.Create(new Product { Id = "2", Brand = "GoPro", Description = "Action camera", Model = "Silver 4k" });
            _productRepository.Create(new Product { Id = "3", Brand = "Sony", Description = "Wireless Headphones", Model = "CH700M" });
            _productRepository.Create(new Product { Id = "4", Brand = "Sony", Description = "Wireless speaker", Model = "SRSXB01" });
            _productRepository.Create(new Product { Id = "5", Brand = "Sony", Description = "TV UHD TV", Model = "X8000G" });
        }

        [Test]
        public void GetWithNoFilters_ReturnsAllFiveProducts()
        {
            var result = _productRepository.Get(null, null, null);
            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public void GetBySonyBrandWithExpectedMatch_ReturnsThreeProducts()
        {
            var result = _productRepository.Get("sony", "", null);
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void GetByWirelessDescriptionWithExpectedMatch_ReturnsTwoProducts()
        {
            var result = _productRepository.Get("", "", "Wireless");
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void GetByNespressoModelWithExpectedMatch_ReturnsOneProduct()
        {
            var result = _productRepository.Get(null, "NESPRESSO", null);
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void GetBySonyBrandAndWirelessDescriptionWithExpectedMatch_ReturnsTwoProducts()
        {
            var result = _productRepository.Get("sony", null, "wireless");
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void GetByAllFiltersWithExpectedMatch_ReturnsOneProducts()
        {
            var result = _productRepository.Get("GoPro", "Silver 4k", "Action camera");
            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void GetByAllFiltersWithUnexpectedMatch_ReturnsNoProducts()
        {
            var result = _productRepository.Get("Canon", "70D", "DSLR camera");
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void GetByAllFiltersWithMatchOnSomeButNotAll_ReturnsNoProducts()
        {
            var result = _productRepository.Get("GoPro", "Mismatch", "camera");
            Assert.AreEqual(0, result.Count);
        }
    }
}