using Grpc.Core;
using Grpc.Net.Client;
using SearchPayment;
using SharedCodeNamePair;

namespace Inst2GrpcPoc.gRPCDmzBridge.Services.Shared
{
    public class PaymentStatusService : SharedCodeNamePairGrpcService.SharedCodeNamePairGrpcServiceBase
    {
        private readonly SharedCodeNamePairGrpcService.SharedCodeNamePairGrpcServiceClient _client;

        public PaymentStatusService(GrpcChannel channel)
        {
            _client = new SharedCodeNamePairGrpcService.SharedCodeNamePairGrpcServiceClient(channel);
        }

        public override async Task<CodeNamePairGrpcReply> GetSharedCodeNamePair(CodeNamePairGrpcRequest request, ServerCallContext context)
        {
            var reply = await _client.GetSharedCodeNamePairAsync(request, deadline: context.Deadline);

            return reply;
        }
    }
}
