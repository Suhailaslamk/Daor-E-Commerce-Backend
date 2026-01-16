using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Application.DTOs.Shipping
{
    public class AddShippingAddressDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address line is required")]
        public string AddressLine { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Postal code must be exactly 6 digits")] public string PostalCode { get; set; } = null!;

        [Required]
        public string Country { get; set; }
    }
}

