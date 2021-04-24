using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using ProductGrpc.Protos;

namespace GrpcProductsClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Waiting for server is running...");
            Thread.Sleep(2000);
            
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new ProductProtoService.ProductProtoServiceClient(channel);

            await GetProductAsync(client);
            await GetAllProductsAsync(client);
            
            Console.ReadLine();
        }
        
        private static async Task GetProductAsync(ProductProtoService.ProductProtoServiceClient client)
        {
            Console.WriteLine("GetProduct started...");
            var response = await client.GetProductAsync(
                new GetProductRequest
                {
                    Id = 1
                });

            Console.WriteLine("GetProductAsync Response: " + response.ToString());
        }
        
        private static async Task GetAllProductsAsync(ProductProtoService.ProductProtoServiceClient client)
        {
            Console.WriteLine("GetAllProducts with C#9 started...");
            using var clientData = client.GetAllProducts(new GetAllProductsRequest());
            await foreach (var responseData in clientData.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine(responseData);
            }
        }
    }
}