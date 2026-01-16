

using Daor_E_Commerce.Application.DTOs.Auth;
using Daor_E_Commerce.Common;

namespace Daor_E_Commerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> Register(RegisterDto dto);
        Task<ApiResponse<object>> Login(LoginDto dto);

        Task<ApiResponse<string>> SendOtp(string email);
        Task<ApiResponse<string>> VerifyOtp(VerifyOtpDto dto);
       
        Task<ApiResponse<string>> ResetPassword(string email, ResetPasswordDto dto);
      

        Task<ApiResponse<string>> RefreshToken(string refreshToken);
        Task<ApiResponse<string>> Logout(int userId);
    }
}
