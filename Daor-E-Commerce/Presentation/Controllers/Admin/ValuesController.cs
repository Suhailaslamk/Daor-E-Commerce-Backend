using Daor_E_Commerce.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Presentation.Controllers.Admin
{
    [Route("api/admin/dashboard")]
    public class AdminDashboardController : AdminControllerBase
    {
        private readonly IAdminDashboardService _service;

        public AdminDashboardController(IAdminDashboardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetStats();
            return StatusCode(result.StatusCode, result);
        }
    }
}
