//using Daor_E_Commerce.Application.DTOs.Auth;
//using Daor_E_Commerce.Application.Interfaces;
//using Daor_E_Commerce.Common;
//using Daor_E_Commerce.Domain;
//using Daor_E_Commerce.Domain.Entities;
//using Daor_E_Commerce.Infrastructure.Data;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace Daor_E_Commerce.Application.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly AppDbContext _context;
//        private readonly IConfiguration _config;

//        private readonly IEmailService _emailService;

//        public AuthService(
//            AppDbContext context,
//            IConfiguration config,
//            IEmailService emailService)
//        {
//            _context = context;
//            _config = config;
//            _emailService = emailService;
//        }
//        // ---------------- REGISTER ----------------
//        public async Task<ApiResponse<string>> Register(RegisterDto dto)
//        {
//            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
//                return new ApiResponse<string>(400, "Email already exists");

//            var user = new User
//            {
//                Name = dto.Name,
//                Email = dto.Email,
//                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
//                Role = "User"
//            };

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            return new ApiResponse<string>(200, "User registered successfully");
//        }

//        // ---------------- LOGIN ----------------
//        public async Task<ApiResponse<object>> Login(LoginDto dto)
//        {
//            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

//            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
//                return new ApiResponse<object>(401, "Invalid email or password");

//            var token = GenerateJwtToken(user);

//            return new ApiResponse<object>(200, "Login successful", new { token });
//        }

//        // ---------------- FORGOT PASSWORD ----------------
//        public async Task<ApiResponse<string>> ForgotPassword(ForgotPasswordDto dto)
//        {
//            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
//            if (user == null)
//                return ApiResponse<string>.Fail("Email not registered", 404);

//            int otpCode = Random.Shared.Next(100000, 999999);

//            _context.OtpCodes.Add(new OtpCode
//            {
//                UserId = user.Id,
//                Code = otpCode,
//                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
//            });

//            await _context.SaveChangesAsync();

//            await _emailService.SendAsync(
//                user.Email,
//                "DAOR Password Reset OTP",
//                $"Your OTP is {otpCode}. It expires in 5 minutes."
//            );

//            return ApiResponse<string>.Success("OTP sent to email");
//        }

//        public async Task<ApiResponse<string>> VerifyOtp(int userId, string otp)
//        {
//            var record = await _context.OtpCodes.FirstOrDefaultAsync(o =>
//                o.UserId == userId &&
//                o.Code == otp &&
//                !o.IsUsed &&
//                o.ExpiresAt > DateTime.UtcNow);

//            if (record == null)
//                return new ApiResponse<string>(400, "Invalid or expired OTP");

//            record.IsUsed = true;
//            await _context.SaveChangesAsync();

//            return new ApiResponse<string>(200, "OTP verified");
//        }

//        // ---------------- RESET PASSWORD ----------------
//        public async Task<ApiResponse<string>> ResetPassword(int userId, ResetPasswordDto dto)
//        {
//            var otp = await _context.OtpCodes.FirstOrDefaultAsync(x =>
//                x.UserId == userId &&
//                x.Code == dto.Otp &&
//                !x.IsUsed &&
//                x.ExpiresAt > DateTime.UtcNow);

//            if (otp == null)
//                return ApiResponse<string>.Fail("Invalid or expired OTP");

//            var user = await _context.Users.FindAsync(userId);
//            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

//            otp.IsUsed = true;
//            await _context.SaveChangesAsync();

//            return ApiResponse<string>.Success("Password reset successful");
//        }


//        // ---------------- PROFILE ----------------
//        public async Task<ApiResponse<UserProfileDto>> GetMyProfile(int userId)
//        {
//            var user = await _context.Users.FindAsync(userId);
//            if (user == null)
//                return new ApiResponse<UserProfileDto>(404, "User not found");

//            return new ApiResponse<UserProfileDto>(200, "Profile fetched", new UserProfileDto
//            {
//                Name = user.Name,
//                Email = user.Email
//            });
//        }

//        public async Task<ApiResponse<string>> UpdateProfile(int userId, UpdateProfileDto dto)
//        {
//            var user = await _context.Users.FindAsync(userId);
//            if (user == null)
//                return new ApiResponse<string>(404, "User not found");

//            user.Name = dto.Name;
//            await _context.SaveChangesAsync();

//            return new ApiResponse<string>(200, "Profile updated successfully");
//        }

//        // ---------------- JWT ----------------
//        private string GenerateJwtToken(User user)
//        {
//            var claims = new[]
//            {
//                new Claim("UserId", user.Id.ToString()),
//                new Claim("EmailId", user.Email),
//                new Claim("Role", user.Role)
//            };

//            var key = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
//            );

//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: _config["Jwt:Issuer"],
//                audience: _config["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(60),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//        public async Task<ApiResponse<string>> SendOtp(int userId)
//        {
//            var user = await _context.Users.FindAsync(userId);
//            if (user == null)
//                return new ApiResponse<string>(404, "User not found");

//            var otp = new OtpCode
//            {
//                UserId = userId,
//                Code = new Random().Next(100000, 999999).ToString(),
//                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
//                IsUsed = false
//            };

//            _context.OtpCodes.Add(otp);
//            await _context.SaveChangesAsync();

//            return new ApiResponse<string>(200, "OTP sent successfully");
//        }
//        public async Task<ApiResponse<string>> Logout(int userId)
//        {
//            // TODO: remove refresh token from DB
//            return new ApiResponse<string>(200, "Logged out successfully");
//        }

//        public async Task<ApiResponse<object>> RefreshToken(string refreshToken)
//        {
//            // TODO: validate refresh token from DB (best practice)
//            return new ApiResponse<object>(200, "Token refreshed", new
//            {
//                token = "NEW_ACCESS_TOKEN"
//            });
//        }
//    }
//}




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




        // ---------------- REGISTER ----------------
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

        // ---------------- LOGIN ----------------
        public async Task<ApiResponse<string>> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return new ApiResponse<string>(401, "Invalid email or password");

            var token = GenerateJwt(user);

            return new ApiResponse<string>(200, "Login successful", token);
        }

        // ---------------- SEND OTP ----------------
        public async Task<ApiResponse<string>> SendOtp(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                return new ApiResponse<string>(404, "Email not registered");

            var otp = new Random().Next(100000, 999999).ToString();

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

        // ---------------- VERIFY OTP ----------------
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


        // ---------------- RESET PASSWORD ----------------
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

        // ---------------- REFRESH TOKEN ----------------
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

        // ---------------- LOGOUT ----------------
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

        // ---------------- JWT GENERATOR ----------------
        private string GenerateJwt(User user)
        {
            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
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
    }
}
