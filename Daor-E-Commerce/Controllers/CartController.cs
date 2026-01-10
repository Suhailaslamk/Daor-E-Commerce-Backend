using Daor_E_Commerce.DTOs;
using Daor_E_Commerce.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Daor_E_Commerce.Controllers
{
    [Authorize] // This protects all methods in this controller
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Helper method to get UserId from the JWT Token
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            int userId = GetUserId(); // Get the ID of the logged-in user
            var result = await _cartService.AddToCartAsync(userId, dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            int userId = GetUserId();
            var cart = await _cartService.GetUserCartAsync(userId);

            if (cart == null) return NotFound("Your cart is empty");
            return Ok(cart);
        }
       
    }
}