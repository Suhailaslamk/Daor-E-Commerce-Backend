using Daor_E_Commerce.DTOs;
using Daor_E_Commerce.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Daor_E_Commerce.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.Register(dto);
            if (result == "Email already exists") return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.Login(dto);
            if (token == null) return Unauthorized("Invalid Email or Password");

            return Ok(new { Token = token });
        }
    }
}