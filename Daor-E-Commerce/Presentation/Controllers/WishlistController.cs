//using Daor_E_Commerce.Application.Interfaces;
//using Daor_E_Commerce.Application.Services;
//using Daor_E_Commerce.Common;
//using Daor_E_Commerce.Domain.Entities;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace Daor_E_Commerce.Presentation.Controllers
//{
//    [Authorize]
//    [ApiController]
//    [Route("api/wishlist")]
//    public class WishlistController : ControllerBase
//    {
//        private readonly IWishlistService _wishlistService;

//        public WishlistController(IWishlistService wishlistService)
//        {
//            _wishlistService = wishlistService;
//        }

//        [HttpPost("toggle/{productId}")]
//        public async Task<IActionResult> Toggle(int productId)
//        {
//            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
//            var result = await _wishlistService.ToggleWishlistAsync(userId, productId);
//            return Ok(new { Message = result });
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetMyWishlist()
//        {
//            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
//            var list = await _wishlistService.GetUserWishlistAsync(userId);
//            return Ok(list);
//        }


//        [HttpDelete("clear")]
//        public async Task<IActionResult> ClearWishlist()
//        {
//            int userId = int.Parse(User.FindFirstValue("UserId"));

//            await _wishlistService.ClearWishlist(userId);

//            return Ok(new ApiResponse<object>(200, "Wishlist cleared successfully"));
//        }

//    }
//}



using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/wishlist")]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _service;

    public WishlistController(IWishlistService service)
    {
        _service = service;
    }

    private int UserId => int.Parse(User.FindFirst("uid")!.Value);

    [HttpPost("toggle/{productId}")]
    public async Task<IActionResult> Toggle(int productId)
    {
        var result = await _service.ToggleWishlist(UserId, productId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetWishlist(UserId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> Clear()
    {
        var result = await _service.ClearWishlist(UserId);
        return StatusCode(result.StatusCode, result);
    }
}
