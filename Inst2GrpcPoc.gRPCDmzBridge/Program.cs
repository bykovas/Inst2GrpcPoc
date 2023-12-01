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

            const string corsPolicy = "_corsAllowAnyOriginPolicy";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, policy =>
                {
                    policy.AllowAnyOrigin()
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

            // Get logger
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            // UseCors must be called before UseRouting and UseEndpoints
            app.UseCors(corsPolicy);

            // Custom middleware for logging CORS preflight requests
            app.Use(async (context, next) =>
            {
                if (context.Request.Method == "OPTIONS")
                {
                    logger.LogInformation("Handling CORS preflight request.");
                }
                // Adding CORS headers here is unnecessary as it's already configured in AddCors
                await next();
            });

            app.UseRouting();

            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<SearchPaymentService>().EnableGrpcWeb().RequireCors(corsPolicy);
                endpoints.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");
            });

            app.Run();
        }
    }
}