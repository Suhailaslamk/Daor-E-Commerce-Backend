using Daor_E_Commerce.Application.DTOs.Payement;
using Daor_E_Commerce.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentsController(IPaymentService service)
        {
            _service = service;
        }

        private int UserId => int.Parse(User.FindFirst("UserId")!.Value);

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            var result = await _service.CreatePayment(UserId, dto);
            return StatusCode(result.StatusCode, result);
        }
    }

}
