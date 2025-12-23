using Microsoft.AspNetCore.Mvc;
using PS_Test.Server.Interfaces;
using PS_Test.Server.Models;

namespace PS_Test.Server.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var product = _productService.GetById(id);

            return product == null 
                ? NotFound() 
                : Ok(product);
        }

        [HttpGet("find")]
        public IActionResult GetByCode([FromQuery] string code)
        {
            var product = _productService.GetByCode(code);

            return product == null
                ? NotFound()
                : Ok(product);
        }

        [HttpPost]
        public IActionResult Add([FromBody] ProductModel product)
        {
            var addedProduct = _productService.Add(product);

            return addedProduct == null
                ? BadRequest()
                : CreatedAtAction(nameof(Add), new { id = addedProduct.Id }, addedProduct);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] ProductModel updatingProduct)
        {
            if (id != updatingProduct.Id)
                return BadRequest("Id in URL and Id in request body must be equal.");

            return _productService.Update(updatingProduct)
                ? Ok()
                : BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _productService.Delete(id);
            return NoContent();
        }
    }
}
