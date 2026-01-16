using Daor_E_Commerce.Application.DTOs.Admin.Category;
using Daor_E_Commerce.Common;



    namespace Daor_E_Commerce.Application.Interfaces.Admin
    {
        public interface IAdminCategoryService
        {
            Task<ApiResponse<int>> Create(CreateCategoryDto dto);
            Task<ApiResponse<string>> Update(UpdateCategoryDto dto);
            Task<ApiResponse<string>> Delete(int categoryId);

            Task<ApiResponse<object>> GetAll();
        }
    }

