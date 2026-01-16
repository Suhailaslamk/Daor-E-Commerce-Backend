


using Daor_E_Commerce.Application.DTOs.Auth;
using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Daor_E_Commerce.Presentation.Controllers
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
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);
            return StatusCode(result.StatusCode, result);
        }

       


        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(ForgotPasswordDto dto)
        {
            var result = await _authService.SendOtp(dto.Email);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        {
            var result = await _authService.VerifyOtp(dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromQuery] string email,
            ResetPasswordDto dto)
        {
            var result = await _authService.ResetPassword( email, dto);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
        {
            var result = await _authService.RefreshToken(dto.RefreshToken);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult MyProfile()
        {
            var userId = User.FindFirstValue("UserId");
            var email = User.FindFirstValue("Email");

            return Ok(new ApiResponse<object>(200, "Profile fetched", new
            {
                UserId = userId,
                Email = email
            }));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var result = await _authService.Logout(userId);

            return StatusCode(result.StatusCode, result);
        }
    }
}
