using Filed.Services.Data.DataModels;

namespace Filed.Services.Contracts
{
    public interface IPaymentService
    {
        void ProcessPayment(Payment p);
    }
}
