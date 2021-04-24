using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcProducts.Data;
using Microsoft.Extensions.Logging;
using ProductGrpc.Protos;

namespace GrpcProducts.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly ProductContext _productContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductContext productContext, ILogger<ProductService> logger)
        {
            _productContext = productContext ?? throw new ArgumentException(nameof(productContext));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        public override Task<Empty> Test(Empty request, ServerCallContext context)
        {
            return base.Test(request, context);
        }

        public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _productContext.Product.FindAsync(request.Id);

            if (product == null)
            {
                // throw an rpc exception
            }

            var productModel = new ProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Status = ProductStatus.Instock,
                CreatedAt = Timestamp.FromDateTime(product.CreatedAt)
            };

            return productModel;
        }
    }
}