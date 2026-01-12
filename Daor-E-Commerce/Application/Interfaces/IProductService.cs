
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Application.DTOs.Products;

namespace Daor_E_Commerce.Application.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponse<object>> GetAll();
        Task<ApiResponse<object>> GetById(int id);
        Task<ApiResponse<object>> GetByCategory(int categoryId);
        Task<ApiResponse<object>> GetPaged(int page, int pageSize);
        Task<ApiResponse<object>> FilterAndSort(ProductFilterDto dto);
        Task<ApiResponse<object>> Search(string search, int page, int pageSize);
    }
}
