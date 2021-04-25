using GrpcShoppingCart.Models;
using Microsoft.EntityFrameworkCore;

namespace GrpcShoppingCart.Data
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options) : base(options)
        {
        }
        
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        
        public DbSet<ShoppingCartItem> ShoppingCartItem { get; set; }
    }
}