using Daor_E_Commerce.Common;
using Daor_E_Commerce.Application.DTOs.Orders;

namespace Daor_E_Commerce.Application.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResponse<string>> CreateOrder(int userId);
        Task<ApiResponse<object>> GetMyOrders(int userId);
        Task<ApiResponse<object>> GetOrderById(int userId, int orderId);
        Task<ApiResponse<string>> CancelOrder(int userId, int orderId);
        Task<ApiResponse<string>> VerifyPayment(int userId, VerifyPaymentDto dto);
    }
}
