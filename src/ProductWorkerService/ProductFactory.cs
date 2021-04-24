using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProductGrpc.Protos;

namespace ProductWorkerService
{
    public class ProductFactory
    {
        private readonly ILogger<ProductFactory> _logger;
        private readonly IConfiguration _configuration;

        public ProductFactory(ILogger<ProductFactory> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public Task<AddProductRequest> Generate()
        {
            var name = _configuration.GetValue<string>("WorkerService:ProductName");
            var productName = $"{name}_{DateTimeOffset.Now}";

            var productResponse = new AddProductRequest
            {
                Product = new ProductModel
                {
                    Name = productName,
                    Description = $"{productName}_Description",
                    Price = new Random().Next(1000),
                    Status = ProductStatus.Instock,
                    CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                }
            };

            return Task.FromResult(productResponse);
        }
   
    }
}