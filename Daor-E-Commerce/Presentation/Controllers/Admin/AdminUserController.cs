using Daor_E_Commerce.Application.DTOs.Admin.Users;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Presentation.Controllers.Admin
{
    [Route("api/admin/users")]
    public class AdminUsersController : AdminControllerBase
    {
        private readonly IAdminUserService _service;

        public AdminUsersController(IAdminUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAll(page, pageSize);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("block")]
        public async Task<IActionResult> Block(BlockUserDto dto)
        {
            var result = await _service.BlockUser(dto.UserId, dto.Block);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}/orders")]
        public async Task<IActionResult> Orders(int id)
        {
            var result = await _service.GetUserOrders(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
