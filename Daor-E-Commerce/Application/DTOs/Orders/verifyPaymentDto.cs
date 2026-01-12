namespace Daor_E_Commerce.Application.DTOs.Orders
{
    public class VerifyPaymentDto
    {
        public int OrderId { get; set; }
        public string PaymentReference { get; set; }
    }
}