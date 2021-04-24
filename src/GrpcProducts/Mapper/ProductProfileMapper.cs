using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using ProductGrpc.Protos;

namespace GrpcProducts.Mapper
{
    public class ProductProfileMapper : Profile
    {
        public ProductProfileMapper()
        {
            CreateMap<Models.Product, ProductModel>()
                .ForMember(
                    dest => dest.CreatedAt, 
                    opt => opt.MapFrom(
                        src => Timestamp.FromDateTime(src.CreatedAt)));
            
            CreateMap<ProductModel, Models.Product>()
                .ForMember(
                    dest => dest.CreatedAt, 
                    opt => opt.MapFrom(
                        src => src.CreatedAt.ToDateTime()));
        }
    }
}