using Grpc.Core;
using Grpc.Net.Client;
using SearchPayment;

namespace Inst2GrpcPoc.gRPCDmzBridge.Services.SearchPayment
{
    public class SearchPaymentService : SearchPaymentGrpcService.SearchPaymentGrpcServiceBase
    {
        private readonly SearchPaymentGrpcService.SearchPaymentGrpcServiceClient _client;

        public SearchPaymentService(GrpcChannel channel)
        {
            _client = new SearchPaymentGrpcService.SearchPaymentGrpcServiceClient(channel);
        }

        public override async Task RetrievePaymentsGrpcStream(SearchPaymentGrpcRequest request, IServerStreamWriter<SearchPaymentGrpcReply> responseStream, ServerCallContext context)
        {
            var call = _client.RetrievePaymentsGrpcStream(request);

            await foreach (var reply in call.ResponseStream.ReadAllAsync())
            {
                await responseStream.WriteAsync(reply);
            }
        }
    }
}
