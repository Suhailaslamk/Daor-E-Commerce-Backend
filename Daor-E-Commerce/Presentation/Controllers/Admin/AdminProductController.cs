using Daor_E_Commerce.Application.DTOs.Admin.Product;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Presentation.Controllers.Admin
{
    [Route("api/admin/products")]
    public class AdminProductsController : AdminControllerBase
    {
        private readonly IAdminProductService _service;

        public AdminProductsController(IAdminProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductDto dto)
        {
            var result = await _service.Add(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto dto)
        {
            var result = await _service.Update(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("toggle/{id}")]
        public async Task<IActionResult> Toggle(int id)
        {
            var result = await _service.ToggleStatus(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
