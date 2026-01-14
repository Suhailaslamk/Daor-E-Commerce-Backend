//using Daor_E_Commerce.Application.Interfaces;
//using Daor_E_Commerce.Application.DTOs.Orders;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace Daor_E_Commerce.Presentation.Controllers
//{
//    [Authorize]
//    [ApiController]
//    [Route("api/orders")]
//    public class OrderController : ControllerBase
//    {
//        private readonly IOrderService _orderService;

//        public OrderController(IOrderService orderService)
//        {
//            _orderService = orderService;
//        }

//        private int UserId => int.Parse(User.FindFirstValue("UserId"));

//        [HttpPost]
//        public async Task<IActionResult> Create()
//            => Ok(await _orderService.CreateOrder(UserId));

//        [HttpGet]
//        public async Task<IActionResult> MyOrders()
//            => Ok(await _orderService.GetMyOrders(UserId));

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//            => Ok(await _orderService.GetOrderById(UserId, id));

//        [HttpPut("cancel/{id}")]
//        public async Task<IActionResult> Cancel(int id)
//            => Ok(await _orderService.CancelOrder(UserId, id));

//        [HttpPost("verify-payment")]
//        [AllowAnonymous] // webhook/payment gateway
//        public async Task<IActionResult> VerifyPayment(VerifyPaymentDto dto)
//            => Ok(await _orderService.VerifyPayment(dto));
//    }
//}


using Daor_E_Commerce.Application.DTOs.Orders;
using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;

    public OrdersController(IOrderService service)
    {
        _service = service;
    }

    private int UserId => int.Parse(User.FindFirst("uid")!.Value);

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        var result = await _service.CreateOrder(UserId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> MyOrders()
    {
        var result = await _service.GetMyOrders(UserId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var result = await _service.GetOrderById(UserId, id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("cancel/{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _service.CancelOrder(UserId, id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("verify-payment")]
    public async Task<IActionResult> VerifyPayment(VerifyPaymentDto dto)
    {
        var result = await _service.VerifyPayment(UserId, dto);
        return StatusCode(result.StatusCode, result);
    }
}
