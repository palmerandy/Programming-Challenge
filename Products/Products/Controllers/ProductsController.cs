using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Products.Models;
using Products.Services;

namespace Products.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Gets all products matching optional filters of brand, model and description.
        /// </summary>
        /// <param name="brand">Optional filter of Product Brand</param>
        /// <param name="model">Optional filter of Product Model</param>
        /// <param name="description">Optional filter of Product Description</param>
        /// <response code="200">Products found</response>
        [ProducesResponseType(200)]
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get([FromQuery]string brand, [FromQuery]string model, [FromQuery]string description)
        {
            return _productService.Get(brand, model, description);
        }

        /// <summary>
        /// Gets a single product by id
        /// </summary>
        /// <param name="id">The Product Id</param>
        /// <response code="200">Product found</response>
        /// <response code="404">Product not found</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public ActionResult<Product> Get(string id)
        {
            var product = _productService.Get(id);
            if(product == null)
            {
                return NotFound();
            }
            return product;
        }


        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Products
        ///     {
        ///        "brand": "Sonos",
        ///        "model": "Play 1",
        ///        "description": "Wireless connected speaker"
        ///     }
        ///
        /// </remarks>
        /// <param name="product">The product data to update</param>
        /// <response code="201">Successful completion of the request, available at action</response>
        /// <response code="400">Validation error</response>
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [HttpPost]
        public ActionResult<Product> Post(ProductRequest product)
        {
            var createdProduct = _productService.Create(product);
            return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     Put /Products/{id}
        ///     {
        ///        "brand": "Electrolux",
        ///        "model": "10 KG Front load",
        ///        "description": "Front loading washing machine"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">The Product Id</param>
        /// <param name="product">The product data to update</param>
        /// <response code="204">Successful completion of the request</response>
        /// <response code="400">Validation error - this API is for updating an existing product</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [HttpPut("{id}")]
        public ActionResult Put(string id, ProductRequest product)
        {
            _productService.Update(id, product, new ModelStateWrapper(ModelState));
            if (ModelState.IsValid)
            {
                return NoContent();
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id">The Product Id</param>
        /// <response code="204">Action has been enacted and no further information is to be supplied</response>
        [ProducesResponseType(204)]
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            _productService.Delete(id);
            return NoContent();
        }
    }
}