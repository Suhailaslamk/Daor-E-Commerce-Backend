using Daor_E_Commerce.Application.DTOs.Products;
using Daor_E_Commerce.Common;

namespace Daor_E_Commerce.Application.Interfaces.IServices
{
    public interface IProductService
    {
        Task<ApiResponse<List<ProductResponseDto>>> GetAllAsync();
        Task<ApiResponse<ProductResponseDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<ProductResponseDto>>> GetByCategoryAsync(int categoryId);
        Task<ApiResponse<List<ProductResponseDto>>> FilterAndSortAsync(ProductFilterDto filter);
        Task<ApiResponse<List<ProductResponseDto>>> SearchAsync(ProductSearchDto dto);

    }
}
