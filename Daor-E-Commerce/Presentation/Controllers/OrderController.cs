

using Daor_E_Commerce.Application.DTOs.Orders;
using Daor_E_Commerce.Application.Interfaces.IServices;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    private int UserId => int.Parse(User.FindFirst("UserId")!.Value);

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto dto)
    {
        var result = await _service.CreateOrder(UserId, dto);
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
