using System.Collections.Generic;
using System.Linq;
using GrpcShoppingCart.Models;

namespace GrpcShoppingCart.Data
{
    public class ShoppingCartContextSeed
    {
        public static void SeedAsync(ShoppingCartContext shoppingCartContext)
        {
            if (!shoppingCartContext.ShoppingCart.Any())
            {
                var shoppingCarts = new List<ShoppingCart>
                {
                    new ShoppingCart
                    {
                        UserName = "fsv",
                        Items = new List<ShoppingCartItem>
                        {
                            new ShoppingCartItem
                            {
                                Quantity = 2,
                                Color = "Black",
                                Price = 699,
                                ProductId = 1,
                                ProductName = "Mi10T"
                            },
                            new ShoppingCartItem
                            {
                                Quantity = 1,
                                Color = "White",
                                Price = 1299,
                                ProductId = 5,
                                ProductName = "MacBook"
                            }
                        }
                    }
                };
                
                shoppingCartContext.ShoppingCart.AddRange(shoppingCarts);
                shoppingCartContext.SaveChanges();
            }
        }
    }
}