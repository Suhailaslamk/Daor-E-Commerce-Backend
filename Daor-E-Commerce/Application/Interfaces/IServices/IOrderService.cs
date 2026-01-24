using Daor_E_Commerce.Application.DTOs.Orders;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Entities;

namespace Daor_E_Commerce.Application.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<ApiResponse<object>> CreateOrder(int userId, CreateOrderDto dto);
        Task<ApiResponse<object>> GetMyOrders(int userId);
        Task<ApiResponse<object>> GetOrderById(int userId, int orderId);
        Task<ApiResponse<string>> CancelOrder(int userId, int orderId);
        Task<ApiResponse<OrderPaymentResponseDto>> VerifyPayment(int userId, VerifyPaymentDto dto);
    }
}
