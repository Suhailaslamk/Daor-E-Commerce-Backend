using Daor_E_Commerce.Application.DTOs.Admin.Category;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Presentation.Controllers.Admin
{
    [Route("api/admin/categories")]
    public class AdminCategoriesController : AdminControllerBase
    {
        private readonly IAdminCategoryService _service;

        public AdminCategoriesController(IAdminCategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            var result = await _service.Create(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryDto dto)
        {
            var result = await _service.Update(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.Delete(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAll();
            return StatusCode(result.StatusCode, result);
        }
    }
}
