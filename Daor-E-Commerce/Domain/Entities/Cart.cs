namespace Daor_E_Commerce.Domain.Entities
{
    public class Cart : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }


        public virtual ICollection<CartItem> CartItems { get; set; }
            = new List<CartItem>();
    }
}
