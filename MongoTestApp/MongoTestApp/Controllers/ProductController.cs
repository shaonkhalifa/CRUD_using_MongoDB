using Microsoft.AspNetCore.Mvc;
using MongoTestApp.Entity;
using MongoTestApp.Services;

namespace MongoTestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("get-all-products")]
        public async Task<IActionResult> GetProducts()
        {
            var data = await _productService.GetProducts();
            return Ok(data);
        }


        [HttpGet("get-single-product")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var data = await _productService.GetProductsById(id);
            return Ok(data);
        }

        [HttpPost("add-new-product")]
        public async Task<IActionResult> AddProduct(Product product)
        {

            await _productService.AddProduct(product);
            return Ok();
        }

        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct(string id, Product product)
        {
            await _productService.UpdateProduct(id, product);
            return Ok();
        }
    }
}
