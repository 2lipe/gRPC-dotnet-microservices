# gRPC in Microservices for Building a high-performance Interservice Communication with .Net 5


**implementations on gRPC in Microservices for Building a high-performance Interservice Communication with .Net 5**  

![Overall Picture of Repository](https://user-images.githubusercontent.com/1147445/98652230-5f66ee80-234c-11eb-9201-8b291b331c9f.png)


### ProductGrpc Server Application
First of all, we are going to develop ProductGrpc project. This will be Asp.Net gRPC Server Web Application and expose apis for Product CRUD operations.

### Product Worker Service
After that, we are going to develop Product Worker Service project for consuming ProductGrpc services. This product worker service project will be the client of ProductGrpc application and generate products and insert bulk product records into Product database by using client streaming gRPC proto services of ProductGrpc application. This operation will be in a time interval and looping as a service application.

### ShoppingCartGrpc Server Application
After that, we are going to develop ShoppingCartGrpc project. This will be asp.net gRPC server web application and expose apis for SC and SC items operations. The grpc services will be create sc and add or remove item into sc.

### ShoppingCart Worker Service
After that, we are going to develop ShoppingCart Worker Service project for consuming ShoppingCartGrpc services. This ShoppingCart worker service project will be the client of both ProductGrpc and ShoppingCartGrpc application. This worker service will read the products from ProductGrpc and create sc and add product items into sc by using gRPC proto services of ProductGrpc and ShoppingCartGrpc application. This operation will be in a time interval and looping as a service application.

### DiscountGrpc Server Application
When adding product item into SC, it will retrieve the discount value and calculate the final price of product. This communication also will be gRPC call with SCGrpc and DiscountGrpc application.

### Identity Server
Also, we are going to develop centralized standalone Authentication Server with implementing IdentityServer4 package and the name of microservice is Identity Server.
Identity Server4 is an open source framework which implements OpenId Connect and OAuth2 protocols for .Net Core.
With IdentityServer, we can provide protect our SC gRPC services with OAuth 2.0 and JWT tokens. SC Worker will get the token before send request to SC Grpc server application.
