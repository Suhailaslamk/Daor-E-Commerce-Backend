using Daor_E_Commerce.Application.DTOs.Auth;
using Daor_E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        // ---------- REGISTER ----------
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.Register(dto);
            return StatusCode(result.StatusCode, result);
        }

        // ---------- LOGIN ----------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);
            return StatusCode(result.StatusCode, result);
        }

        // ---------- MY PROFILE ----------
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> MyProfile()
        {
            int userId = int.Parse(User.FindFirstValue("UserId"));
            var result = await _authService.GetMyProfile(userId);
            return StatusCode(result.StatusCode, result);
        }

        // ---------- UPDATE PROFILE ----------
        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            int userId = int.Parse(User.FindFirstValue("UserId"));
            var result = await _authService.UpdateProfile(userId, dto);
            return StatusCode(result.StatusCode, result);
        }

        // ---------- SEND OTP ----------
        [Authorize]
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp()
        {
            int userId = int.Parse(User.FindFirstValue("UserId"));
            var result = await _authService.SendOtp(userId);
            return StatusCode(result.StatusCode, result);
        }

        // ---------- VERIFY OTP ----------
        [Authorize]
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        {
            int userId = int.Parse(User.FindFirstValue("UserId"));
            var result = await _authService.VerifyOtp(userId, dto.Otp);
            return StatusCode(result.StatusCode, result);
        }

        // ---------- RESET PASSWORD ----------
        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            int userId = int.Parse(User.FindFirstValue("UserId"));
            var result = await _authService.ResetPassword(userId, dto);
            return StatusCode(result.StatusCode, result);
        }

        // ---------- REFRESH TOKEN ----------
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
        {
            var result = await _authService.RefreshToken(dto.RefreshToken);
            return StatusCode(result.StatusCode, result);
        }

        // ---------- LOGOUT ----------
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            int userId = int.Parse(User.FindFirstValue("UserId"));
            var result = await _authService.Logout(userId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
