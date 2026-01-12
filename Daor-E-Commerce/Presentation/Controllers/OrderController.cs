using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Daor_E_Commerce.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int UserId => int.Parse(User.FindFirstValue("UserId"));

        [HttpPost]
        public async Task<IActionResult> Create()
            => Ok(await _orderService.CreateOrder(UserId));

        [HttpGet]
        public async Task<IActionResult> MyOrders()
            => Ok(await _orderService.GetMyOrders(UserId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _orderService.GetOrderById(UserId, id));

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
            => Ok(await _orderService.CancelOrder(UserId, id));

        [HttpPost("verify-payment")]
        [AllowAnonymous] // webhook/payment gateway
        public async Task<IActionResult> VerifyPayment(VerifyPaymentDto dto)
            => Ok(await _orderService.VerifyPayment(dto));
    }
}
