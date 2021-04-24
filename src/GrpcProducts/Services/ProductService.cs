using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcProducts.Data;
using GrpcProducts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGrpc.Protos;
using ProductStatus = ProductGrpc.Protos.ProductStatus;

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

        public override async Task GetAllProducts(GetAllProductsRequest request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            var productList = await _productContext.Product.ToListAsync();

            foreach (var product in productList)
            {
                var productModel = new ProductModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Status = ProductStatus.Instock,
                    CreatedAt = Timestamp.FromDateTime(product.CreatedAt)
                };

                await responseStream.WriteAsync(productModel);
            }
        }

        public override async Task<ProductModel> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            var product = new Product
            {
                Id = request.Product.Id,
                Name = request.Product.Name,
                Description = request.Product.Description,
                Price = request.Product.Price,
                Status = Models.ProductStatus.INSTOCK,
                CreatedAt = request.Product.CreatedAt.ToDateTime()
            };

            _productContext.Product.Add(product);
            await _productContext.SaveChangesAsync();
            
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