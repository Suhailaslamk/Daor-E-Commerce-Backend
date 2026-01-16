using Daor_E_Commerce.Common;
using Daor_E_Commerce.Application.DTOs.Admin.Product;

namespace Daor_E_Commerce.Application.Interfaces.Admin

{
    public interface IAdminProductService
    {
        Task<ApiResponse<int>> Create(CreateProductDto dto);
        Task<ApiResponse<string>> Update(UpdateProductDto dto);
        Task<ApiResponse<string>> Delete(int productId);

        Task<ApiResponse<object>> GetAll();
        Task<ApiResponse<object>> GetById(int id);
        Task<ApiResponse<string>> ToggleStatus(int productId);
    }
}

        