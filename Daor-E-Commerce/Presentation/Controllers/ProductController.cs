using Daor_E_Commerce.Application.DTOs.Products;
using Daor_E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var result = await _service.GetByCategoryAsync(categoryId);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("filter-sort")]
        public async Task<IActionResult> FilterSort([FromQuery] ProductFilterDto filter)
        {
            var result = await _service.FilterAndSortAsync(filter);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] ProductSearchDto dto)
        {
            var result = await _service.SearchAsync(dto);
            return StatusCode(result.StatusCode, result);
        }


    }
}
