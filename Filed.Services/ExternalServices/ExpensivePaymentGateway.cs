using Filed.Services.Contracts;
using System;

namespace Filed.Services.ExternalServices
{
    public class ExpensivePaymentGateway : IExpensivePaymentGateway
    {
        public bool IsAvailable()
        {
            return new Random().Next(0, 3) <= 1;
        }

        public int ProcessPayment()
        {
            //Randomness of Faliure set to 1 out of 3
            return (new Random().Next(1, 3) > 2) ? 0 : 1;
        }
    }
}
