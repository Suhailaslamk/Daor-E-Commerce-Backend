using Daor_E_Commerce.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Daor_E_Commerce.Data
{
   
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
            {
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Cart> Carts { get; set; }
            public DbSet<CartItem> CartItems { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<OrderItem> OrderItems { get; set; }
            public DbSet<Wishlist> Wishlists { get; set; }
        }
    }


