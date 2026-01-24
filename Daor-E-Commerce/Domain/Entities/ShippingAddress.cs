using Daor_E_Commerce.Domain.Entities;

namespace Daor_E_Commerce.Domain.Entities
{
    public class ShippingAddress : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }

        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public bool IsActive { get; set; } = false;


       
    }
    }


