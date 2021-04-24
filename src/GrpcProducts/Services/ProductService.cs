using System;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcProducts.Data;
using GrpcProducts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductGrpc.Protos;

namespace GrpcProducts.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly ProductContext _productContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductContext productContext, IMapper mapper, ILogger<ProductService> logger)
        {
            _productContext = productContext ?? throw new ArgumentException(nameof(productContext));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
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
            
            var productModel = _mapper.Map<ProductModel>(product);

            return productModel;
        }

        public override async Task GetAllProducts(GetAllProductsRequest request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            var productList = await _productContext.Product.ToListAsync();

            foreach (var product in productList)
            {
                var productModel = _mapper.Map<ProductModel>(product);
                
                await responseStream.WriteAsync(productModel);
            }
        }

        public override async Task<ProductModel> AddProduct(AddProductRequest request, ServerCallContext context)
        {
            
            var product = _mapper.Map<Product>(request.Product);
            
            _productContext.Product.Add(product);
            await _productContext.SaveChangesAsync();
            
            var productModel = _mapper.Map<ProductModel>(product);
            
            return productModel;
        }

        public override async Task<ProductModel> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            var product = _mapper.Map<Product>(request.Product);

            bool isExist = await _productContext.Product.AnyAsync(p => p.Id == product.Id);
            if (!isExist)
            {
                // throw an rpc exception
            }

            _productContext.Entry(product).State = EntityState.Modified;

            try
            {
                await _productContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new Exception(e.Message);
            }

            var productModel = _mapper.Map<ProductModel>(product);

            return productModel;
        }

        public override async Task<DeleteProductResponse> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            var product = await _productContext.Product.FindAsync(request.Id);
            
            if (product == null)
            {
                // throw an rpc exception
            }

            _productContext.Product.Remove(product);
            var deleteCount = await _productContext.SaveChangesAsync();

            var response = new DeleteProductResponse
            {
                Success = deleteCount > 0
            };

            return response;
        }
    }
}