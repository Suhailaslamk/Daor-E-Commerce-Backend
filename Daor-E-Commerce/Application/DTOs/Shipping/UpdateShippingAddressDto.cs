using System.ComponentModel.DataAnnotations;

namespace Daor_E_Commerce.Application.DTOs.Shipping
{
    public class UpdateShippingAddressDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string AddressLine { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
