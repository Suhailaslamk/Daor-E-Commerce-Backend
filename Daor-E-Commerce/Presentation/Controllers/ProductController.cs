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

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAll());

        [HttpGet("getby/{id}")]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _service.GetById(id));

        [HttpGet("getcatby/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
            => Ok(await _service.GetByCategory(categoryId));

        [HttpGet("paged")]
        public async Task<IActionResult> Paged(int page = 1, int pageSize = 10)
            => Ok(await _service.GetPaged(page, pageSize));

        [HttpGet("search")]
        public async Task<IActionResult> Search(string search, int page = 1, int pageSize = 10)
            => Ok(await _service.Search(search, page, pageSize));

        [HttpGet("filter-sort")]
        public async Task<IActionResult> FilterSort([FromQuery] ProductFilterDto dto)
            => Ok(await _service.FilterAndSort(dto));
    }
}
