using Grpc.Core;
using Inst2GrpcPoc.DataAccess.MONITOR;
using SearchPayment;

namespace Inst2GrpcPoc.gRPCDataBridge.Services.SearchPayment
{
    public class SearchPaymentService : SearchPaymentGrpcService.SearchPaymentGrpcServiceBase
    {
        private readonly OrdersInstPac2 _ordersInstPac2 = new();

        public override async Task RetrievePaymentsGrpcStream(SearchPaymentGrpcRequest request, IServerStreamWriter<SearchPaymentGrpcReply> responseStream, ServerCallContext context)
        {
            await _ordersInstPac2.GetPayments(request, responseStream, context);
        }
    }
}
