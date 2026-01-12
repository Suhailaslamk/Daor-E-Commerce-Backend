using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Application.Services;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Daor_E_Commerce.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/wishlist")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("toggle/{productId}")]
        public async Task<IActionResult> Toggle(int productId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _wishlistService.ToggleWishlistAsync(userId, productId);
            return Ok(new { Message = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyWishlist()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var list = await _wishlistService.GetUserWishlistAsync(userId);
            return Ok(list);
        }

        
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearWishlist()
        {
            int userId = int.Parse(User.FindFirstValue("UserId"));

            await _wishlistService.ClearWishlist(userId);

            return Ok(new ApiResponse<object>(200, "Wishlist cleared successfully"));
        }

    }
}