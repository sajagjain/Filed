using Filed.Services.Contracts;
using System;

namespace Filed.Services.ExternalServices
{
    public class CheapPaymentGateway : ICheapPaymentGateway
    {
        public int ProcessPayment()
        {
            //Randomness of Faliure set to 1 out of 4
            return (new Random().Next(1, 4) > 3) ? 0 : 1;
        }
    }
}
