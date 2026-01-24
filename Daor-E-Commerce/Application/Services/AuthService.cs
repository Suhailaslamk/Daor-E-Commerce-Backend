

using Daor_E_Commerce.Application.DTOs.Auth;
using Daor_E_Commerce.Application.Interfaces.IServices;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Entities;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Daor_E_Commerce.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;

        public AuthService(
            AppDbContext context,
            IConfiguration config,
            IEmailService emailService)
        {
            _context = context;
            _config = config;
            _emailService = emailService;
        }

        private static string NormalizeEmail(string email)
        {
            return email
                .Trim()
                .Replace(" ", "")
                .ToLowerInvariant();
        }

        public async Task<ApiResponse<string>> Register(RegisterDto dto)
        {
            if (dto == null)
                return new ApiResponse<string>(400, "Invalid request");

            //dto.FullName = dto.FullName?.Trim();
            //dto.Password = dto.Password?.Trim();
            dto.Email = dto.Email?.Trim();

            if (dto.Email.Contains(" "))
                return new ApiResponse<string>(400, "Email must not contain spaces");

            if (string.IsNullOrWhiteSpace(dto.FullName))
                return new ApiResponse<string>(400, "Full name is required");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return new ApiResponse<string>(400, "Email is required");

            if (!new EmailAddressAttribute().IsValid(dto.Email))
                return new ApiResponse<string>(400, "Invalid email format");

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 8)
                return new ApiResponse<string>(400, "Password must be at least 8 characters");

            if (!System.Text.RegularExpressions.Regex.IsMatch(
                    dto.Password,
                    @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&]).+$"))
                return new ApiResponse<string>(400, "Password is too weak");

            if (!Regex.IsMatch(dto.FullName, @"^[A-Za-z]+(?:[ .'-][A-Za-z]+)*$"))
                return new ApiResponse<string>(400, "Full name must contain only letters and valid separators");
            var normalizedEmail = NormalizeEmail(dto.Email);

            bool exists = await _context.Users
                .AnyAsync(u => u.Email == normalizedEmail);

            if (exists)
                return new ApiResponse<string>(409, "Email already exists");

            var user = new User
            {
                FullName = dto.FullName,
                Email = normalizedEmail, 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsEmailVerified = false,
                Role = UserRole.User
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>(201, "User registered successfully");
        }

        public async Task<ApiResponse<object>> Login(LoginDto dto)
        {
            if (dto == null)
                return new ApiResponse<object>(400, "Invalid request");

            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
                return new ApiResponse<object>(400, "Email and password are required");

            var normalizedEmail = NormalizeEmail(dto.Email);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            if (user == null)
                return new ApiResponse<object>(401, "Invalid email or password");

            

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
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
            if (string.IsNullOrWhiteSpace(email))
                return new ApiResponse<string>(400, "Email is required");

            var normalizedEmail = NormalizeEmail(email);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            if (user == null)
                return new ApiResponse<string>(404, "User not found");

            var otp = new Random().Next(100000, 999999).ToString();

            _context.OtpCodes.Add(new OtpCode
            {
                UserId = user.Id,
                Code = otp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            });

            await _context.SaveChangesAsync();

            await _emailService.SendAsync(
                user.Email,
                "Your OTP Code",
                $"Your OTP is {otp}. It expires in 5 minutes."
            );

            return new ApiResponse<string>(200, "OTP sent successfully");
        }

        
        public async Task<ApiResponse<string>> VerifyOtp(VerifyOtpDto dto)
        {
            var normalizedEmail = NormalizeEmail(dto.Email);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            if (user == null)
                return new ApiResponse<string>(404, "User not found");

            var otp = await _context.OtpCodes.FirstOrDefaultAsync(o =>
                o.UserId == user.Id &&
                o.Code == dto.Otp &&
                !o.IsUsed &&
                o.ExpiresAt > DateTime.UtcNow);

            if (otp == null)
                return new ApiResponse<string>(400, "Invalid or expired OTP");

            otp.IsUsed = true;
            user.IsEmailVerified = true;

            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "OTP verified successfully");
        }

        
        private string GenerateJwt(User user)
        {
            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("UserName",user.FullName.ToString()),
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

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
        public async Task<ApiResponse<string>> ResetPassword(string email, ResetPasswordDto dto)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(dto.Otp) ||
                string.IsNullOrWhiteSpace(dto.NewPassword))
                return new ApiResponse<string>(400, "Email, OTP and new password are required");

            var normalizedEmail = NormalizeEmail(email);

            if (!System.Text.RegularExpressions.Regex.IsMatch(dto.Otp, @"^\d{6}$"))
                return new ApiResponse<string>(400, "OTP must be a 6-digit number");

            if (dto.NewPassword.Length < 8 ||
                !System.Text.RegularExpressions.Regex.IsMatch(
                    dto.NewPassword,
                    @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&]).+$"))
                return new ApiResponse<string>(400, "Password is too weak");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == normalizedEmail);

            if (user == null)
                return new ApiResponse<string>(404, "User not found");

            var otp = await _context.OtpCodes.FirstOrDefaultAsync(o =>
                o.UserId == user.Id &&
                o.Code == dto.Otp &&
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
            if (string.IsNullOrWhiteSpace(refreshToken))
                return new ApiResponse<string>(400, "Refresh token is required");

            var stored = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.Token == refreshToken &&
                    !x.IsRevoked &&
                    x.ExpiresAt > DateTime.UtcNow);

            if (stored == null)
                return new ApiResponse<string>(401, "Invalid or expired refresh token");

            var newJwt = GenerateJwt(stored.User);

            return new ApiResponse<string>(200, "Token refreshed successfully", newJwt);
        }
        public async Task<ApiResponse<string>> Logout(int userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync();

            if (!tokens.Any())
                return new ApiResponse<string>(200, "Already logged out");

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }

            await _context.SaveChangesAsync();

            return new ApiResponse<string>(200, "Logged out successfully");
        }



    }
}
