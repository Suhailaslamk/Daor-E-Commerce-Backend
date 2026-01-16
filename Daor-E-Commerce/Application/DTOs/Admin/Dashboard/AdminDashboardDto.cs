namespace Daor_E_Commerce.Application.DTOs.Admin.Dashboard
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int ActiveProducts { get; set; }

        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }

        public int TodayOrders { get; set; }
    }
}
