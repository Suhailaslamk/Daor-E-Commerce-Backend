using Daor_E_Commerce.Application.DTOs.Admin.Product;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Presentation.Controllers.Admin
{
    [Route("api/admin/orders")]
    public class AdminOrdersController : AdminControllerBase
    {
        private readonly IAdminOrderService _service;

        public AdminOrdersController(IAdminOrderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetById(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusDto dto)
        {
            var result = await _service.UpdateStatus(dto);
            return StatusCode(result.StatusCode, result);
        }
    }
}
