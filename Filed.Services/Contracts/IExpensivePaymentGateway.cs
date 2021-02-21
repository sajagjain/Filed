namespace Filed.Services.Contracts
{
    public interface IExpensivePaymentGateway
    {
        public int ProcessPayment();
        public bool IsAvailable();
    }
}
