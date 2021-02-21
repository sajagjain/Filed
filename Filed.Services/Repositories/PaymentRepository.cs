using Filed.Services.Contracts;
using Filed.Services.Data;
using Filed.Services.Data.DataModels;
using System.Linq;

namespace Filed.Services.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        readonly PaymentContext _paymentContext;

        public PaymentRepository(PaymentContext paymentContext)
        {
            this._paymentContext = paymentContext;
        }

        public Payment Get(int Id)
        {
            return _paymentContext.Payments.Where(a => a.Id == Id).FirstOrDefault();
        }

        public int Save(Payment p)
        {
            _paymentContext.Payments.Add(p);
            return _paymentContext.SaveChanges();
        }
    }
}
