using Daor_E_Commerce.Application.DTOs.Admin;
using Daor_E_Commerce.Application.DTOs.Admin.Product;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Presentation.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/products")]
    public class AdminProductsController : ControllerBase
    {
        private readonly IAdminProductService _service;

        public AdminProductsController(IAdminProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var result = await _service.Create(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto dto)
        {
            var result = await _service.Update(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> Toggle(int id)
        {
            var result = await _service.ToggleStatus(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            string? search,
            int page = 1,
            int pageSize = 10)
        {
            var result = await _service.GetAll(search, page, pageSize);
            return StatusCode(result.StatusCode, result);
        }
    }
}
