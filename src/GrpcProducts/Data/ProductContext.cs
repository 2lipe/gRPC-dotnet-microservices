using GrpcProducts.Models;
using Microsoft.EntityFrameworkCore;

namespace GrpcProducts.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }
        
        public DbSet<Product> Product { get; set; }
    }
}