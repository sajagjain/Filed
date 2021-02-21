using Filed.Services.Contracts;
using Filed.Services.Data.DataModels;
using Filed.Services.ExternalServices;
using Polly;
using Polly.Retry;
using System;

namespace Filed.Services
{
    public class PaymentService : IPaymentService
    {
        readonly IPaymentRepository _paymentRepository;
        readonly IPaymentStateRepository _paymentStateRepository;
        readonly IExpensivePaymentGateway _expensivePaymentGateway;
        readonly ICheapPaymentGateway _cheapPaymentGateway;
        readonly IExpensivePaymentGateway _premiumPaymentGateway;
        readonly RetryPolicy<PremiumPaymentService> _retryPolicy;

        public PaymentService(IPaymentRepository paymentRepository
            , IPaymentStateRepository paymentStateRepository
            , IExpensivePaymentGateway expensivePaymentGateway
            , ICheapPaymentGateway cheapPaymentGateway
            , IExpensivePaymentGateway premiumPaymentGateway)
        {
            this._paymentRepository = paymentRepository;
            this._paymentStateRepository = paymentStateRepository;
            this._expensivePaymentGateway = expensivePaymentGateway;
            this._cheapPaymentGateway = cheapPaymentGateway;
            this._premiumPaymentGateway = premiumPaymentGateway;
            this._retryPolicy = Policy<PremiumPaymentService>.Handle<Exception>().Retry(3);
        }

        public void ProcessPayment(Payment p)
        {
            //If upto 20 euros then use Cheap Payment Gateway
            if (p.Amount <= 20)
            {
                var result = _cheapPaymentGateway.ProcessPayment();
                SavePaymentDetails(result, p);
            }
            else if (p.Amount > 20 && p.Amount <= 500)
            {
                //Check if Expensive Gateway is Available
                if (_expensivePaymentGateway.IsAvailable())
                {
                    var result = _expensivePaymentGateway.ProcessPayment();
                    SavePaymentDetails(result, p);
                }
                else
                {
                    var result = _cheapPaymentGateway.ProcessPayment();
                    SavePaymentDetails(result, p);
                }
            }
            else if (p.Amount > 500)
            {

                //This will be retried 3 times
                _retryPolicy.Execute(() =>
                {
                    var result = _premiumPaymentGateway.ProcessPayment();
                    if (result == 0)
                    {
                        //Save Failed Status
                        SavePaymentDetails(result, p);
                        //Throw Payment Failed Exception for Polly Retry
                        throw new Exception("Premium Payment Failed");
                    }
                    SavePaymentDetails(result, p);
                    return null;
                });
            }
        }

        public void SavePaymentDetails(int result, Payment p)
        {
            SavePaymentDetails("processing", p);
            if (result == 1)
            {
                SavePaymentDetails("processed", p);
            }
            else if (result == 0)
            {
                SavePaymentDetails("failed", p);
            }
        }

        public void SavePaymentDetails(string v, Payment p)
        {
            //Check for p.Id value
            if (p != null && p.Id == 0)
            {
                _paymentRepository.Save(p);
            }

            PaymentState paymentState = new PaymentState()
            {
                PayState = v,
                PaymentId = p.Id,
                Created = DateTime.UtcNow
            };

            _paymentStateRepository.Save(paymentState);
        }
    }
}
