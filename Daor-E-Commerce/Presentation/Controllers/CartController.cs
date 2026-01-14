//using Daor_E_Commerce.Application.DTOs.Cart;
//using Daor_E_Commerce.Application.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace Daor_E_Commerce.Presentation.Controllers
//{
//    [Authorize]
//    [ApiController]
//    [Route("api/cart")]
//    public class CartController : ControllerBase
//    {
//        private readonly ICartService _cartService;

//        public CartController(ICartService cartService)
//        {
//            _cartService = cartService;
//        }

//        private int UserId => int.Parse(User.FindFirstValue("UserId"));

//        [HttpGet]
//        public async Task<IActionResult> GetCart()
//            => Ok(await _cartService.GetMyCart(UserId));

//        [HttpPost("add")]
//        public async Task<IActionResult> Add(AddToCartDto dto)
//            => Ok(await _cartService.AddToCart(UserId, dto));

//        [HttpPut("update")]
//        public async Task<IActionResult> Update(UpdateCartItemDto dto)
//            => Ok(await _cartService.UpdateCartItem(UserId, dto));

//        [HttpDelete("remove/{productId}")]
//        public async Task<IActionResult> Remove(int productId)
//            => Ok(await _cartService.RemoveCartItem(UserId, productId));

//        [HttpDelete("clear")]
//        public async Task<IActionResult> Clear()
//            => Ok(await _cartService.ClearCart(UserId));
//    }
//}



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

    private int UserId => int.Parse(User.FindFirst("uid")!.Value);

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
