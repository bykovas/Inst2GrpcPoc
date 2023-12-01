using Grpc.Net.Client;
using Inst2GrpcPoc.gRPCDmzBridge.Services.SearchPayment;

namespace Inst2GrpcPoc.gRPCDmzBridge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            const string corsPolicy = "_corsAllowAnyOriginPolicy";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding", "Access-Control-Allow-Origin");
                });
            });

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            // Add services to the container.
            builder.Services.AddGrpc();

            // Hardcoded internal gPRC
            var internalGrpcServerAddress = "http://localhost:5268";
            builder.Services.AddSingleton<GrpcChannel>(s => GrpcChannel.ForAddress(internalGrpcServerAddress));

            var app = builder.Build();

            app.UseCors();
            app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

            // Configure the HTTP request pipeline.
            app.MapGrpcService<SearchPaymentService>().EnableGrpcWeb().RequireCors(corsPolicy);
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}