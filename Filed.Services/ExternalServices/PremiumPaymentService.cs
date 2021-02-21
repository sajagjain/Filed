using Filed.Services.Contracts;
using System;

namespace Filed.Services.ExternalServices
{
    public class PremiumPaymentService : IExpensivePaymentGateway
    {
        public bool IsAvailable()
        {
            //Always Available as Premium Payment Service
            return true;
        }

        public int ProcessPayment()
        {
            //Randomness of Faliure set to 1 out of 3
            return (new Random().Next(1, 3) > 2) ? 0 : 1;
        }
    }
}
