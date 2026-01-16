using Daor_E_Commerce.Application.DTOs.Admin.Dashboard;
using Daor_E_Commerce.Common;

namespace Daor_E_Commerce.Application.Interfaces.Admin
{
    public interface IAdminDashboardService
    {
        Task<ApiResponse<AdminDashboardDto>> GetStats();
    }
}
