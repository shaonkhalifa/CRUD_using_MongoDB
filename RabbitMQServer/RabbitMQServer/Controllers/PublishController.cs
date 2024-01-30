using Microsoft.AspNetCore.Mvc;
using RabbitMQServer.Entity;
using RabbitMQServer.Sevices;

namespace RabbitMQServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly RabbitMQService<Product> _rabbitMQService;

        public PublishController(RabbitMQService<Product> rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        [HttpPost]
        public async Task<IActionResult> PublishProduct([FromBody] Product product)
        {
            try
            {
                _rabbitMQService.PublishMessage(product, "product-routing", "DirectExchange");
                return Ok("Message published successfully");
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Failed to publish message: " + ex.Message);
            }
        }
    }
}
