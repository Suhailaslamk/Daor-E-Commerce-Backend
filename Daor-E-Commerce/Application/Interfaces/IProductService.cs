
//using Daor_E_Commerce.Common;
//using Daor_E_Commerce.Application.DTOs.Products;

//namespace Daor_E_Commerce.Application.Interfaces
//{
//    public interface IProductService
//    {
//        Task<ApiResponse<List<ProductResponseDto>>> GetAllAsync();
//        Task<ApiResponse<ProductResponseDto>> GetByIdAsync(int id);
//        Task<ApiResponse<List<ProductResponseDto>>> GetByCategoryAsync(int categoryId);
//        Task<ApiResponse<object>> GetPaged(int page, int pageSize);
//        Task<ApiResponse<object>> FilterSort(ProductFilterDto dto);
//        Task<ApiResponse<object>> Search(string search, int page, int pageSize);
//    }
//}

using Daor_E_Commerce.Application.DTOs.Products;
using Daor_E_Commerce.Common;

namespace Daor_E_Commerce.Application.Interfaces
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
