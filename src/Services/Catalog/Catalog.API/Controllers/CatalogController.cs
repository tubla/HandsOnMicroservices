using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts() 
        {
            var product = await _productRepository.GetProducts();
            return Ok(product);
        }

        [HttpGet("{id:length(24)}",Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var result = await _productRepository.GetProduct(id);
            if(result == null)
            {
                _logger.LogError($"Product with id : {id} not found");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("[action]/{categoryName}",Name = "GetProductByCategory")] // GetProductByCategory/xyz
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string categoryName)
        {
            var result = await _productRepository.GetProductByCategory(categoryName);
            if (result == null)
            {
                _logger.LogError($"Product with category name : {categoryName} not found");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody]Product product)
        {
           await _productRepository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id },product);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await _productRepository.DeleteProduct(id));
        }
    }
}
