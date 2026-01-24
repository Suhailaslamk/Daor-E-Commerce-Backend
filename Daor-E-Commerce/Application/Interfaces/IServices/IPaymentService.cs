using Daor_E_Commerce.Application.DTOs.Payement;
using Daor_E_Commerce.Common;

namespace Daor_E_Commerce.Application.Interfaces.IServices
{
    public interface IPaymentService
    {
        Task<ApiResponse<object>> CreatePayment(int userId, CreatePaymentDto dto);
    }
}
