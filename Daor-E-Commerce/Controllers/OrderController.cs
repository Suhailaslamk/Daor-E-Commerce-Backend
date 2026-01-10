using Daor_E_Commerce.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Daor_E_Commerce.Controllers
{
    [Authorize] // Only logged-in users can access orders
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // This helper gets the User ID from the JWT Token
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            int userId = GetUserId();
            var result = await _orderService.PlaceOrderAsync(userId);

            if (result.Contains("empty"))
                return BadRequest(new { message = result });

            return Ok(new { message = result });
        }

        [HttpGet("my-history")]
        public async Task<IActionResult> GetMyOrders()
        {
            int userId = GetUserId();
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }
    }
}