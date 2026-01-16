


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

    private int UserId =>
        int.Parse(User.FindFirst("UserId")!.Value);

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
