using SearchPayment;
using Grpc.Core;

namespace Inst2GrpcPoc.DataAccess.MONITOR
{
    public class OrdersInstPac2
    {
        public async Task GetPayments(
            SearchPaymentGrpcRequest request, 
            IServerStreamWriter<SearchPaymentGrpcReply> responseStream, 
            ServerCallContext context)
        {
            var faker = new Bogus.Faker();

            for (int i = 0; i < request.NumberOfPayments; i++)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    break;
                }

                var fakePayment = new SearchPaymentGrpcReply
                {
                    Id = i,
                    Sender = faker.Finance.Bic(),
                    Receiver = faker.Finance.Bic(),
                    Amount = faker.Random.Double(100, 10000),
                    Timestamp = faker.Date.Recent(30).ToString("o"),
                };

                await responseStream.WriteAsync(fakePayment);

                // Delay for simulating data retrieval latency, if needed.
                await Task.Delay(500); // 500 ms delay
            }
        }
    }
}
