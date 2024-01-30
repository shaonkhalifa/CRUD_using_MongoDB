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

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProductWithFiltering(string? kyeword)
        {

            int page = 2;
            int pagesize = 5;
            var data = await _productService.GetProductWithPaging(kyeword, page, pagesize);
            return Ok(data);
        }

        [HttpGet("get-sum")]
        public async Task<IActionResult> GetSum(string? kyeword)
        {
            var data = await _productService.GetSumResult(kyeword);
            return Ok(data);
        }

        [HttpGet("get-date-products")]
        public async Task<IActionResult> DateFilter(string startDate, string endDate)
        {
            var data = await _productService.DateFilter(startDate, endDate);
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
