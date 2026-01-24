using Daor_E_Commerce.Application.DTOs.Payement;
using Daor_E_Commerce.Application.Interfaces.IServices;
using Daor_E_Commerce.Common;
using Daor_E_Commerce.Domain.Enums;
using Daor_E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Daor_E_Commerce.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<object>> CreatePayment(int userId, CreatePaymentDto dto)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o =>
                    o.Id == dto.OrderId &&
                    o.UserId == userId);

            if (order == null)
                return new ApiResponse<object>(404, "Order not found");

            if (order.Status == OrderStatus.Paid)
                return new ApiResponse<object>(400, "Order already paid");

            var paymentIntentId = Guid.NewGuid().ToString();

            order.PaymentIntentId = paymentIntentId;

            await _context.SaveChangesAsync();

            return new ApiResponse<object>(200, "Payment initiated", new
            {
                order.Id,
                Amount = order.TotalAmount,
                Currency = "INR",
                PaymentIntentId = paymentIntentId
            });
        }
    }

}
