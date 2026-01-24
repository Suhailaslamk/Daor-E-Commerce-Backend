using Daor_E_Commerce.Application.DTOs.Admin.Product;
using Daor_E_Commerce.Application.Interfaces.Admin;
using Daor_E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Presentation.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/products")]
    public class AdminProductsController : AdminControllerBase
    {
        private readonly IAdminProductService _adminService;

        public AdminProductsController(
            IAdminProductService adminService)
           
        {
            _adminService = adminService;
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
        {
            var result = await _adminService.Create(dto);
            return StatusCode(result.StatusCode, result);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _adminService.GetById(id);
            return StatusCode(result.StatusCode, result);
        }

        
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUpdate(
            int id,
            [FromForm] UpdateProductPatchDto dto)
        {
            var result = await _adminService.PatchUpdate(id, dto);
            return StatusCode(result.StatusCode, result);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll(
            string? search,
            int page = 1,
            int pageSize = 10)
        {
            var result = await _adminService.GetAll(search, page, pageSize);
            return StatusCode(result.StatusCode, result);
        }
    }
}
