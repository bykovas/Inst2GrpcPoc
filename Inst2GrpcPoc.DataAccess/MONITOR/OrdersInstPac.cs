namespace Inst2GrpcPoc.DataAccess.MONITOR
{
    public class OrdersInstPac
    {
        public IEnumerable<KeyValuePair<string, string>> GetPaymentTypes(string language)
        {
            var faker = new Bogus.Faker();

            var paymentTypes = Enumerable.Range(0, 10).Select(_ =>
                new KeyValuePair<string, string>(
                    faker.Finance.Account(),
                    faker.Commerce.ProductName()
                )).ToList();

            return paymentTypes;
        }
    }
}
