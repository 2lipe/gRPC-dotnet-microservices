using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProductGrpc.Protos;

namespace ProductWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly ProductFactory _productFactory;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, ProductFactory productFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _productFactory = productFactory;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Thread.Sleep(2000);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var interval = _configuration.GetValue<int>("WorkerService:TaskInterval");
                var serverUrl = _configuration.GetValue<string>("WorkerService:ServerUrl");
                
                using var channel = GrpcChannel.ForAddress(serverUrl);
                var client = new ProductProtoService.ProductProtoServiceClient(channel);
                
                _logger.LogInformation("AddProductAsync started...");
                var addProductResponse = await client.AddProductAsync(await _productFactory.Generate());
                _logger.LogInformation("AddProduct Response: {product}", addProductResponse.ToString());
                
                await Task.Delay(interval, stoppingToken);
            }
        }
    }
}