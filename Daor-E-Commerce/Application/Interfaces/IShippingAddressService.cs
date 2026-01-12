using Daor_E_Commerce.Application.DTOs.Shipping;
using Daor_E_Commerce.Common;

namespace Daor_E_Commerce.Application.Interfaces
{
    public interface IShippingAddressService
    {
        Task<ApiResponse<object>> Add(int userId, AddShippingAddressDto dto);
        Task<ApiResponse<object>> GetMyAddresses(int userId);
        Task<ApiResponse<object>> Update(int userId, int id, UpdateShippingAddressDto dto);
        Task<ApiResponse<object>> SetActive(int userId, int id);
        Task<ApiResponse<object>> Delete(int userId, int id);
    }
}
