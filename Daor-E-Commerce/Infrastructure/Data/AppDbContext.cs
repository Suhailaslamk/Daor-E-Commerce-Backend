using Daor_E_Commerce.Domain;
using Daor_E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Daor_E_Commerce.Infrastructure.Data
{
   
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
            {
            }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
    .HasOne(o => o.ShippingAddress)
    .WithMany() // address not dependent on order lifecycle
    .HasForeignKey(o => o.ShippingAddressId)
    .OnDelete(DeleteBehavior.Restrict);
        }
            public DbSet<User> Users { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Cart> Carts { get; set; }
            public DbSet<CartItem> CartItems { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderItem> OrderItems { get; set; }
            public DbSet<WishlistItem> WishlistItems { get; set; }
            public DbSet<RefreshToken> RefreshTokens { get; set; }
            public DbSet<OtpCode> OtpCodes { get; set; }
            public DbSet<ShippingAddress> ShippingAddresses { get; set; }
            public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }

    }
}


