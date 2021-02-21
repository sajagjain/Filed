using Filed.Services.Contracts;
using Filed.Services.Data;
using Filed.Services.Data.DataModels;

namespace Filed.Services.Repositories
{
    public class PaymentStateRepository : IPaymentStateRepository
    {
        readonly PaymentContext _paymentContext;

        public PaymentStateRepository(PaymentContext paymentContext)
        {
            this._paymentContext = paymentContext;
        }

        public int Save(PaymentState t)
        {
            _paymentContext.PaymentStates.Add(t);
            return _paymentContext.SaveChanges();
        }
    }
}
