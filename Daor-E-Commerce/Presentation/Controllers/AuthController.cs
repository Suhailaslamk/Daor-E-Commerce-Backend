//using Daor_E_Commerce.Application.DTOs.Auth;
//using Daor_E_Commerce.Application.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace Daor_E_Commerce.Presentation.Controllers
//{
//    [ApiController]
//    [Route("api/auth")]
//    public class AuthController : ControllerBase
//    {
//        private readonly IAuthService _authService;

//        public AuthController(IAuthService authService)
//        {
//            _authService = authService;
//        }

//        // ---------- REGISTER ----------
//        [HttpPost("register")]
//        public async Task<IActionResult> Register(RegisterDto dto)
//        {
//            var result = await _authService.Register(dto);
//            return StatusCode(result.StatusCode, result);
//        }

//        // ---------- LOGIN ----------
//        [HttpPost("login")]
//        public async Task<IActionResult> Login(LoginDto dto)
//        {
//            var result = await _authService.Login(dto);
//            return StatusCode(result.StatusCode, result);
//        }

//        // ---------- MY PROFILE ----------
//        [Authorize]
//        [HttpGet("me")]
//        public async Task<IActionResult> MyProfile()
//        {
//            int userId = int.Parse(User.FindFirstValue("UserId"));
//            var result = await _authService.GetMyProfile(userId);
//            return StatusCode(result.StatusCode, result);
//        }

//        // ---------- UPDATE PROFILE ----------
//        [Authorize]
//        [HttpPut("update-profile")]
//        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
//        {
//            int userId = int.Parse(User.FindFirstValue("UserId"));
//            var result = await _authService.UpdateProfile(userId, dto);
//            return StatusCode(result.StatusCode, result);
//        }

//        // ---------- SEND OTP ----------
//        [Authorize]
//        [HttpPost("send-otp")]
//        public async Task<IActionResult> SendOtp()
//        {
//            int userId = int.Parse(User.FindFirstValue("UserId"));
//            var result = await _authService.SendOtp(userId);
//            return StatusCode(result.StatusCode, result);
//        }

//        // ---------- VERIFY OTP ----------
//        [Authorize]
//        [HttpPost("verify-otp")]
//        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
//        {
//            int userId = int.Parse(User.FindFirstValue("UserId"));
//            var result = await _authService.VerifyOtp(userId, dto.Otp);
//            return StatusCode(result.StatusCode, result);
//        }

//        // ---------- RESET PASSWORD ----------
//        [Authorize]
//        [HttpPost("reset-password")]
//        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
//        {
//            int userId = int.Parse(User.FindFirstValue("UserId"));
//            var result = await _authService.ResetPassword(userId, dto);
//            return StatusCode(result.StatusCode, result);
//        }

//        // ---------- REFRESH TOKEN ----------
//        [HttpPost("refresh-token")]
//        public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
//        {
//            var result = await _authService.RefreshToken(dto.RefreshToken);
//            return StatusCode(result.StatusCode, result);
//        }

//        // ---------- LOGOUT ----------
//        [Authorize]
//        [HttpPost("logout")]
//        public async Task<IActionResult> Logout()
//        {
//            int userId = int.Parse(User.FindFirstValue("UserId"));
//            var result = await _authService.Logout(userId);
//            return StatusCode(result.StatusCode, result);
//        }
//    }
//}


using Daor_E_Commerce.Application.DTOs.Auth;
using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Common;
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

        // ---------------- REGISTER ----------------
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.Register(dto);
            return StatusCode(result.StatusCode, result);
        }

        // ---------------- LOGIN ----------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);
            return StatusCode(result.StatusCode, result);
        }

        //// ---------------- SEND OTP ----------------
        //[HttpPost("send-otp")]
        //public async Task<IActionResult> SendOtp([FromBody] ForgotPasswordDto dto)
        //{
        //    var result = await _authService.SendOtp(dto.Email);
        //    return StatusCode(result.StatusCode, result);
        //}

        ////[HttpPost("send-otp")]
        ////public async Task<IActionResult> SendOtp(string email)
        ////{
        ////    var result = await _authService.SendOtp(email);
        ////    return StatusCode(result.StatusCode, result);
        ////}

        ////[HttpPost("verify-otp")]
        ////public async Task<IActionResult> VerifyOtp(int userId, string otp)
        ////{
        ////    var result = await _authService.VerifyOtp(userId, otp);
        ////    return StatusCode(result.StatusCode, result);
        ////}














        //// ---------------- VERIFY OTP ----------------
        //[HttpPost("verify-otp")]
        //public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        //{
        //    var result = await _authService.VerifyOtp(dto);
        //    return StatusCode(result.StatusCode, result);
        //}

        //// ---------------- RESET PASSWORD ----------------
        //[HttpPut("reset-password/{userId}")]
        //public async Task<IActionResult> ResetPassword(int userId, ResetPasswordDto dto)
        //{
        //    var result = await _authService.ResetPassword(userId, dto);
        //    return StatusCode(result.StatusCode, result);
        //}


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

        // ---------------- REFRESH TOKEN ----------------
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
        {
            var result = await _authService.RefreshToken(dto.RefreshToken);
            return StatusCode(result.StatusCode, result);
        }

        // ---------------- MY PROFILE ----------------
        [Authorize]
        [HttpGet("me")]
        public IActionResult MyProfile()
        {
            var userId = User.FindFirstValue("UserId");
            var email = User.FindFirstValue(ClaimTypes.Email);

            return Ok(new ApiResponse<object>(200, "Profile fetched", new
            {
                UserId = userId,
                Email = email
            }));
        }

        // ---------------- LOGOUT ----------------
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
