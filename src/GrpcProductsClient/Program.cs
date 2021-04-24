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
            
            // GetProductAsync
            Console.WriteLine("GetProductAsync started...");
            var response = await client.GetProductAsync(
                new GetProductRequest
                {
                    Id = 1
                });

            Console.WriteLine("GetProductAsync Response: " + response.ToString());
            
            // GetAllProducts
            // Console.WriteLine("GetAllProducts started...");
            // using (var clientData =  client.GetAllProducts(new GetAllProductsRequest()))
            // {
            //     while (await clientData.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
            //     {
            //         var currentProduct = clientData.ResponseStream.Current;
            //         Console.WriteLine(currentProduct);
            //     }
            // }
            
            // GetAllProducts with C#9
            Console.WriteLine("GetAllProducts with C#9 started...");
            using var clientData = client.GetAllProducts(new GetAllProductsRequest());
            await foreach (var responseData in clientData.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine(responseData);
            }

            
            Console.ReadLine();
        }
    }
}