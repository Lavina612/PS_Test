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
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productService.GetById(id);

            return product == null 
                ? NotFound() 
                : Ok(product);
        }

        [HttpGet("find")]
        public async Task<IActionResult> GetByCode([FromQuery] string code)
        {
            var product = await _productService.GetByCode(code);

            return product == null
                ? NotFound()
                : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductModel product)
        {
            var addedProduct = await _productService.Add(product);

            return addedProduct == null
                ? BadRequest()
                : CreatedAtAction(nameof(Add), new { id = addedProduct.Id }, addedProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductModel updatingProduct)
        {
            if (id != updatingProduct.Id)
                return BadRequest("Id in URL and Id in request body must be equal.");

            return await _productService.Update(updatingProduct)
                ? Ok()
                : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.Delete(id);
            return NoContent();
        }
    }
}
