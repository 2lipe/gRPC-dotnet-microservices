using System;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
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
            await AddProductAsync(client);
            await UpdateProductAsync(client);
            await DeleteProductAsync(client);
            
            Console.ReadLine();
        }
        
        private static async Task GetProductAsync(ProductProtoService.ProductProtoServiceClient client)
        {
            Console.WriteLine("GetProductAsync started...");
            var response = await client.GetProductAsync(
                new GetProductRequest
                {
                    Id = 1
                });

            Console.WriteLine("GetProductAsync Response: " + response.ToString());
        }
        
        private static async Task GetAllProductsAsync(ProductProtoService.ProductProtoServiceClient client)
        {
            Console.WriteLine("GetAllProductsAsync with C#9 started...");
            using var clientData = client.GetAllProducts(new GetAllProductsRequest());
            await foreach (var responseData in clientData.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine(responseData);
            }
        }
        
        private static async Task AddProductAsync(ProductProtoService.ProductProtoServiceClient client)
        {
            Console.WriteLine("AddProductAsync started...");
            var addProductResponse = await client.AddProductAsync(
                new AddProductRequest
                {
                    Product = new ProductModel
                    {
                        Name = "Redmi",
                        Description = "New Redmi smartphone",
                        Price = 599,
                        Status = ProductStatus.Instock,
                        CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                    }
                });
            
            Console.WriteLine("AddProductAsync Response: " + addProductResponse.ToString());
        }
        
        private static async Task UpdateProductAsync(ProductProtoService.ProductProtoServiceClient client)
        {
            Console.WriteLine("UpdateProductAsync started...");
            var updateProductResponse = await client.UpdateProductAsync(
                new UpdateProductRequest
                {
                    Product = new ProductModel
                    {
                        Id = 1,
                        Name = "Mi10T",
                        Description = "New Xiaomi 10T awesome smartphone",
                        Price = 699,
                        Status = ProductStatus.Instock,
                        CreatedAt = Timestamp.FromDateTime(DateTime.UtcNow)
                    }
                });
            
            Console.WriteLine("UpdateProductAsync Response: " + updateProductResponse.ToString());

        }
        
        private static async Task DeleteProductAsync(ProductProtoService.ProductProtoServiceClient client)
        {
            Console.WriteLine("DeleteProductAsync started...");
            var deleteProductResponse = await client.DeleteProductAsync(
                new DeleteProductRequest
                {
                    Id = 2
                });

            Console.WriteLine("DeleteProductAsync Response: " + deleteProductResponse.ToString());
        }
    }
}