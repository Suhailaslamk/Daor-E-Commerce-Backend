using Daor_E_Commerce.Application.DTOs.Shipping;
using Daor_E_Commerce.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Daor_E_Commerce.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/shipping-address")]
    public class ShippingAddressController : ControllerBase
    {
        private readonly IShippingAddressService _service;

        public ShippingAddressController(IShippingAddressService service)
        {
            _service = service;
        }

        private int UserId => int.Parse(User.FindFirstValue("UserId"));

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddShippingAddressDto dto)
            => Ok(await _service.Add(UserId, dto));

        [HttpGet("my")]
        public async Task<IActionResult> My()
            => Ok(await _service.GetMyAddresses(UserId));

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, UpdateShippingAddressDto dto)
            => Ok(await _service.Update(UserId, id, dto));

        [HttpPut("set-active/{id}")]
        public async Task<IActionResult> SetActive(int id)
            => Ok(await _service.SetActive(UserId, id));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _service.Delete(UserId, id));
    }
}
