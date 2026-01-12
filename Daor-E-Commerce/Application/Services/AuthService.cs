using Daor_E_Commerce.Application.DTOs.Auth;
using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Daor_E_Commerce.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ---------------- REGISTER ----------------
        public async Task<ApiResponse<string>> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return new ApiResponse<string>(400, "Email already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "User registered successfully");
        }

        // ---------------- LOGIN ----------------
        public async Task<ApiResponse<object>> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return new ApiResponse<object>(401, "Invalid email or password");

            var token = GenerateJwtToken(user);

            return new ApiResponse<object>(200, "Login successful", new { token });
        }

        // ---------------- FORGOT PASSWORD ----------------
        public async Task<ApiResponse<string>> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return new ApiResponse<string>(404, "Email not registered");

            var otp = new OtpCode
            {
                UserId = user.Id,
                Code = new Random().Next(100000, 999999).ToString(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            _context.OtpCodes.Add(otp);
            await _context.SaveChangesAsync();

            // TODO: Send OTP via Email/SMS
            return new ApiResponse<string>(200, "OTP sent successfully");
        }
        public async Task<ApiResponse<string>> VerifyOtp(int userId, string otp)
        {
            var record = await _context.OtpCodes.FirstOrDefaultAsync(o =>
                o.UserId == userId &&
                o.Code == otp &&
                !o.IsUsed &&
                o.ExpiresAt > DateTime.UtcNow);

            if (record == null)
                return new ApiResponse<string>(400, "Invalid or expired OTP");

            record.IsUsed = true;
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "OTP verified");
        }

        // ---------------- RESET PASSWORD ----------------
        public async Task<ApiResponse<string>> ResetPassword(int userId, ResetPasswordDto dto)
        {
            var otp = await _context.OtpCodes.FirstOrDefaultAsync(o =>
                o.UserId == userId &&
                o.Code == dto.Otp &&
                !o.IsUsed &&
                o.ExpiresAt > DateTime.UtcNow);

            if (otp == null)
                return new ApiResponse<string>(400, "Invalid or expired OTP");

            var user = await _context.Users.FindAsync(userId);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            otp.IsUsed = true;
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Password reset successful");
        }

        // ---------------- PROFILE ----------------
        public async Task<ApiResponse<UserProfileDto>> GetMyProfile(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new ApiResponse<UserProfileDto>(404, "User not found");

            return new ApiResponse<UserProfileDto>(200, "Profile fetched", new UserProfileDto
            {
                Name = user.Name,
                Email = user.Email
            });
        }

        public async Task<ApiResponse<string>> UpdateProfile(int userId, UpdateProfileDto dto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new ApiResponse<string>(404, "User not found");

            user.Name = dto.Name;
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Profile updated successfully");
        }

        // ---------------- JWT ----------------
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("EmailId", user.Email),
                new Claim("Role", user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<ApiResponse<string>> SendOtp(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return new ApiResponse<string>(404, "User not found");

            var otp = new OtpCode
            {
                UserId = userId,
                Code = new Random().Next(100000, 999999).ToString(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };

            _context.OtpCodes.Add(otp);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "OTP sent successfully");
        }
        public async Task<ApiResponse<string>> Logout(int userId)
        {
            // TODO: remove refresh token from DB
            return new ApiResponse<string>(200, "Logged out successfully");
        }

        public async Task<ApiResponse<object>> RefreshToken(string refreshToken)
        {
            // TODO: validate refresh token from DB (best practice)
            return new ApiResponse<object>(200, "Token refreshed", new
            {
                token = "NEW_ACCESS_TOKEN"
            });
        }
    }
}
