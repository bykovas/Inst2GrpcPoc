using Grpc.Net.Client;
using Inst2GrpcPoc.gRPCDmzBridge.Services.SearchPayment;
using Microsoft.Extensions.Logging;

namespace Inst2GrpcPoc.gRPCDmzBridge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            const string corsPolicy = "_corsAllowBlazorOriginPolicy";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, policy =>
                {
                    policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
                });
            });

            // Add services to the container.
            builder.Services.AddGrpc();

            // Hardcoded internal gRPC server address
            var internalGrpcServerAddress = "http://localhost:5268";
            builder.Services.AddSingleton<GrpcChannel>(s => GrpcChannel.ForAddress(internalGrpcServerAddress));

            var app = builder.Build();

            app.UseRouting();
            
            app.UseGrpcWeb();

            app.UseCors();
            //app.UseHttpsRedirection();

#pragma warning disable ASP0014 // Suggest using top level route registrations
            app.UseEndpoints(ep =>
            {
                ep.MapGrpcService<SearchPaymentService>().EnableGrpcWeb().RequireCors(corsPolicy);
                ep.MapGet("/",
                    () =>
                        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            });
#pragma warning restore ASP0014 // Suggest using top level route registrations

            app.Run();
        }
    }
}