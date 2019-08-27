using System.ComponentModel.DataAnnotations;

namespace Products.Models
{
    public class ProductRequest
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Brand { get; set; }
    }

    public class Product
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string Model { get; set; }

        public string Brand { get; set; }
    }
}
