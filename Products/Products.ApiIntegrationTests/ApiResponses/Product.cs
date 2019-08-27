namespace Products.ApiIntegrationTests.ApiResponses
{
    public class Product
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
    }
    public class RequestProduct
    {
        public string Description { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
    }
}