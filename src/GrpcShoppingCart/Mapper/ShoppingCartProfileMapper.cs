using AutoMapper;
using ShoppingCartGrpc.Protos;

namespace GrpcShoppingCart.Mapper
{
    public class ShoppingCartProfileMapper : Profile
    {
        public ShoppingCartProfileMapper()
        {
            CreateMap<Models.ShoppingCart, ShoppingCartModel>().ReverseMap();
            CreateMap<ShoppingCartModel, Models.ShoppingCart>().ReverseMap();
        }
    }
}