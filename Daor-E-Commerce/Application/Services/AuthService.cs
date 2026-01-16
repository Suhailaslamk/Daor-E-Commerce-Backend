


using Daor_E_Commerce.Application.DTOs.Auth;
using Daor_E_Commerce.Application.Interfaces;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;



namespace Daor_E_Commerce.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AuthService(AppDbContext context, IConfiguration config , IEmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService; 
        }




        public async Task<ApiResponse<string>> Register(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
                return new ApiResponse<string>(400, "Email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(201, "User registered successfully");
        }

        public async Task<ApiResponse<object>> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return new ApiResponse<object>(401, "Invalid email or password");

            var accessToken = GenerateJwt(user);
            var refreshToken = GenerateRefreshToken();

            _context.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            });

            await _context.SaveChangesAsync();

            return new ApiResponse<object>(200, "Login successful", new
            {
                accessToken,
                refreshToken
            });
        }

        public async Task<ApiResponse<string>> SendOtp(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return new ApiResponse<string>(404, "Email not registered");

            var otp = new Random().Next(100000, 999999).ToString();
            Console.WriteLine($"OTP = {otp}");

            _context.OtpCodes.Add(new OtpCode
            {
                UserId = user.Id,
                Code = otp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            });

            await _context.SaveChangesAsync();

            await _emailService.SendAsync(
                user.Email,
                "Your OTP Code",
                $"Your OTP is {otp}. It expires in 5 minutes."
            );

            return new ApiResponse<string>(200, "OTP sent to email");
        }

        public async Task<ApiResponse<string>> VerifyOtp(VerifyOtpDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null)
                return new ApiResponse<string>(404, "User not found");

            var otp = await _context.OtpCodes
                .FirstOrDefaultAsync(o =>
                    o.UserId == user.Id &&
                    o.Code == dto.Otp.ToString() &&
                    !o.IsUsed &&
                    o.ExpiresAt > DateTime.UtcNow);

            if (otp == null)
                return new ApiResponse<string>(400, "Invalid or expired OTP");

            otp.IsUsed = true;
            user.IsEmailVerified = true;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "OTP verified successfully");
        }


        public async Task<ApiResponse<string>> ResetPassword(string email, ResetPasswordDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return new ApiResponse<string>(404, "User not found");

            var otp = await _context.OtpCodes
                .FirstOrDefaultAsync(o =>
                    o.UserId == user.Id &&
                    o.Code == dto.Otp.ToString() &&
                    !o.IsUsed &&
                    o.ExpiresAt > DateTime.UtcNow);

            if (otp == null)
                return new ApiResponse<string>(400, "Invalid or expired OTP");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            otp.IsUsed = true;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Password reset successful");
        }

        public async Task<ApiResponse<string>> RefreshToken(string refreshToken)
        {
            var stored = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.Token == refreshToken &&
                    !x.IsRevoked &&
                    x.ExpiresAt > DateTime.UtcNow);

            if (stored == null)
                return new ApiResponse<string>(401, "Invalid refresh token");

            var newJwt = GenerateJwt(stored.User);

            return new ApiResponse<string>(200, "Token refreshed", newJwt);
        }

        public async Task<ApiResponse<string>> Logout(int userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
                token.IsRevoked = true;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Logged out successfully");
        }

        private string GenerateJwt(User user)
        {
            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())             };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["Jwt:ExpiresInMinutes"])
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

    }
}
