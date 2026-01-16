using Daor_E_Commerce.Domain.Enums;
using System.ComponentModel.DataAnnotations;


namespace Daor_E_Commerce.Application.DTOs.Admin.Order
{
public class UpdateOrderStatusDto
{
    [Required]
    public int OrderId { get; set; }

    [Required]
    public OrderStatus Status { get; set; }
}
}