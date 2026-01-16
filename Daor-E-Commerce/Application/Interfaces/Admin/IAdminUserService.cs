
using Daor_E_Commerce.Common;

namespace Daor_E_Commerce.Application.Interfaces.Admin
{
    public interface IAdminUserService
    {
        Task<ApiResponse<object>> GetAll(int page, int pageSize);
        Task<ApiResponse<object>> GetById(int userId);
        Task<ApiResponse<string>> BlockUser(int userId, bool block);
        Task<ApiResponse<object>> GetUserOrders(int userId);
    }
}
