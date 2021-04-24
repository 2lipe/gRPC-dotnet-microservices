using System;
using System.Collections.Generic;
using System.Linq;
using GrpcProducts.Models;

namespace GrpcProducts.Data
{
    public class ProductContextSeed
    {
        public static void SeedAsync(ProductContext product)
        {
            if (!product.Product.Any())
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        Id = 1,
                        Name = "Mi10T",
                        Description = "New Xiaomi Phone Mi10T",
                        Price = 699,
                        Status = ProductStatus.INSTOCK,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Playstation 5",
                        Description = "New Game Platform",
                        Price = 999,
                        Status = ProductStatus.INSTOCK,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Xbox Series X",
                        Description = "New Awesome Game Platform",
                        Price = 999,
                        Status = ProductStatus.INSTOCK,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "IPhoneX",
                        Description = "Apple smartphone",
                        Price = 499,
                        Status = ProductStatus.NONE,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "MacBook",
                        Description = "Apple computer",
                        Price = 1299,
                        Status = ProductStatus.LOW,
                        CreatedAt = DateTime.UtcNow
                    },
                };
                
                product.Product.AddRange(products);
                product.SaveChanges();
            }
        }
    }
}