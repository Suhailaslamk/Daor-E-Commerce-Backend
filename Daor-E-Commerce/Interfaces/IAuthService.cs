using Daor_E_Commerce.DTOs;

namespace Daor_E_Commerce.Interfaces
{
    public interface IAuthService
    {
        // A service that handles registration and returns a message
        Task<string> Register(RegisterDto dto);

        // A service that handles login and returns a JWT Token
        Task<string> Login(LoginDto dto);
    }
}