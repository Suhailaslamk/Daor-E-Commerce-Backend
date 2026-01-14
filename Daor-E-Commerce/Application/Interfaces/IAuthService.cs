//using Daor_E_Commerce.Application.DTOs.Auth;
//using Daor_E_Commerce.Common;

//public interface IAuthService
//{
//    Task<ApiResponse<string>> Register(RegisterDto dto);
//    Task<ApiResponse<object>> Login(LoginDto dto);

//    Task<ApiResponse<UserProfileDto>> GetMyProfile(int userId);
//    Task<ApiResponse<string>> UpdateProfile(int userId, UpdateProfileDto dto);

//    Task<ApiResponse<string>> SendOtp(int userId);
//    Task<ApiResponse<string>> VerifyOtp(int userId, string otp);

//    Task<ApiResponse<string>> ForgotPassword(ForgotPasswordDto dto);
//    Task<ApiResponse<string>> ResetPassword(int userId, ResetPasswordDto dto);

//    Task<ApiResponse<object>> RefreshToken(string refreshToken);
//    Task<ApiResponse<string>> Logout(int userId);
//}


using Daor_E_Commerce.Application.DTOs.Auth;
using Daor_E_Commerce.Common;

namespace Daor_E_Commerce.Application.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> Register(RegisterDto dto);
        Task<ApiResponse<string>> Login(LoginDto dto);

        Task<ApiResponse<string>> SendOtp(string email);
        Task<ApiResponse<string>> VerifyOtp(VerifyOtpDto dto);
       
        Task<ApiResponse<string>> ResetPassword(string email, ResetPasswordDto dto);
      

        Task<ApiResponse<string>> RefreshToken(string refreshToken);
        Task<ApiResponse<string>> Logout(int userId);
    }
}
