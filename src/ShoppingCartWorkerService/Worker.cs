using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingCartGrpc.Protos;

namespace ShoppingCartWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Waiting for server is running...");
            Thread.Sleep(2000);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                var interval = _configuration.GetValue<int>("WorkerService:TaskInterval");
                
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var scUrl = _configuration.GetValue<string>("WorkerService:ShoppingCartServerUrl");
                using var scChannel = GrpcChannel.ForAddress(scUrl);
                var scClient = new ShoppingCartProtoService.ShoppingCartProtoServiceClient(scChannel);

                var scModel = await GetOrCreateShoppingCartAsync(scClient);
                
                await Task.Delay(interval, stoppingToken);
            }
        }

        private async Task<ShoppingCartModel> GetOrCreateShoppingCartAsync(ShoppingCartProtoService.ShoppingCartProtoServiceClient scClient)
        {
            ShoppingCartModel shoppingCartModel;

            var userName = _configuration.GetValue<string>("WorkerService:UserName");

            try
            {
                _logger.LogInformation("GetShoppingCartAsync started...");
                shoppingCartModel = await scClient.GetShoppingCartAsync(
                    new GetShoppingCartRequest
                    {
                        Username = userName
                    });
                _logger.LogInformation($"GetShoppingCartAsync Response: {shoppingCartModel}", shoppingCartModel);
            }
            catch (RpcException e)
            {
                if (e.StatusCode == StatusCode.NotFound)
                {
                    _logger.LogInformation("CreateShoppingCartAsync started...");
                    shoppingCartModel = await scClient.CreateShoppingCartAsync(
                        new ShoppingCartModel
                        {
                            Username = userName
                        });
                    _logger.LogInformation($"CreateShoppingCartAsync Response: {shoppingCartModel}", shoppingCartModel);
                }
                else
                {
                    throw new Exception(e.Message);
                }
            }

            return shoppingCartModel;
        }
    }
}