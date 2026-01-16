
using Daor_E_Commerce.Application.DTOs.Cart;
using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _service;

    public CartController(ICartService service)
    {
        _service = service;
    }

    private int UserId => int.Parse(User.FindFirst("UserId")!.Value);

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddToCartDto dto)
    {
        var result = await _service.AddToCart(UserId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update(UpdateCartItemDto dto)
    {
        var result = await _service.UpdateCart(UserId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("remove/{productId}")]
    public async Task<IActionResult> Remove(int productId)
    {
        var result = await _service.RemoveFromCart(UserId, productId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetCart(UserId);
        return StatusCode(result.StatusCode, result);
    }
}
