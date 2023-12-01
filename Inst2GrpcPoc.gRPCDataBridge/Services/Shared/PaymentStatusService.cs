using Grpc.Core;
using Inst2GrpcPoc.DataAccess.MONITOR;
using SharedCodeNamePair;

namespace Inst2GrpcPoc.gRPCDataBridge.Services.Shared
{
    public class PaymentStatusService : SharedCodeNamePairGrpcService.SharedCodeNamePairGrpcServiceBase
    {
        private readonly OrdersInstPac _ordersInstPac = new();

        public override async Task<CodeNamePairGrpcReply> GetSharedCodeNamePair(CodeNamePairGrpcRequest request, ServerCallContext context)
        {
            var language = request.Language;
            var paymentTypes = _ordersInstPac.GetPaymentTypes(language);

            var reply = new CodeNamePairGrpcReply
            {
                Pairs = { paymentTypes.Select(pt => new CodeNamePairGrpc { Code = pt.Key, Name = pt.Value }) }
            };

            return await Task.FromResult(reply);
        }
    }
}