using Daor_E_Commerce.Common;
using Daor_E_Commerce.Application.DTOs.Admin.Order;

namespace Daor_E_Commerce.Application.Interfaces.Admin
{
    public interface IAdminOrderService
    {
        Task<ApiResponse<object>> GetAll();
        Task<ApiResponse<object>> GetById(int orderId);
        Task<ApiResponse<string>> UpdateStatus(UpdateOrderStatusDto dto);
    }

}

