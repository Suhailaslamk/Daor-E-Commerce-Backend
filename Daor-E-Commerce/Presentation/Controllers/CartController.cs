using Daor_E_Commerce.Application.DTOs.Cart;
using Daor_E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Daor_E_Commerce.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int UserId => int.Parse(User.FindFirstValue("UserId"));

        [HttpGet]
        public async Task<IActionResult> GetCart()
            => Ok(await _cartService.GetMyCart(UserId));

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddToCartDto dto)
            => Ok(await _cartService.AddToCart(UserId, dto));

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateCartItemDto dto)
            => Ok(await _cartService.UpdateCartItem(UserId, dto));

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> Remove(int productId)
            => Ok(await _cartService.RemoveCartItem(UserId, productId));

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
            => Ok(await _cartService.ClearCart(UserId));
    }
}
