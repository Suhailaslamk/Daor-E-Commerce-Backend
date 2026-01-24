using Daor_E_Commerce.Common;
using Daor_E_Commerce.Application.DTOs.Admin.Product;

namespace Daor_E_Commerce.Application.Interfaces.Admin

{
    public interface IAdminProductService
    {
        Task<ApiResponse<ProductResponseDto>> Create(CreateProductDto dto);
        Task<ApiResponse<string>> PatchUpdate(
                int id,
                UpdateProductPatchDto dto
            );
        Task<ApiResponse<ProductResponseDto>> GetById(int id);

        Task<ApiResponse<string>> Delete(int productId);

        Task<ApiResponse<string>> ToggleStatus(int productId);
        Task<ApiResponse<PagedResponse<ProductListItemDto>>> GetAll(string? search, int page, int pageSize);



    }
}

