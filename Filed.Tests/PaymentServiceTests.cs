using Bogus;
using Filed.Services;
using Filed.Services.Contracts;
using Filed.Services.Data.DataModels;
using Moq;
using Xunit;

namespace Filed.Tests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepository = new Mock<IPaymentRepository>();
        private readonly Mock<IPaymentStateRepository> _paymentStateRepository = new Mock<IPaymentStateRepository>();
        private readonly Mock<IExpensivePaymentGateway> _expensivePaymentGateway = new Mock<IExpensivePaymentGateway>();
        private readonly Mock<ICheapPaymentGateway> _cheapPaymentGateway = new Mock<ICheapPaymentGateway>();
        private readonly Mock<IExpensivePaymentGateway> _premiumPaymentGateway = new Mock<IExpensivePaymentGateway>();
        private readonly Faker<Payment> paymentFaker;
        private readonly IPaymentService _paymentService;

        public PaymentServiceTests()
        {
            paymentFaker = new Faker<Payment>()
                 .RuleFor(a => a.CardHolder, x => x.Name.FullName())
                 .RuleFor(a => a.CreditCardNumber, x => x.Finance.CreditCardNumber())
                 .RuleFor(a => a.ExpirationDate, x => x.Date.Future());

            _paymentService = new PaymentService(_paymentRepository.Object
                , _paymentStateRepository.Object
                , _expensivePaymentGateway.Object
                , _cheapPaymentGateway.Object
                , _premiumPaymentGateway.Object);
        }

        [Fact]
        public void ProcessPayment_ShouldProcessUsingCheapPaymentGateway_WhenAmountLessThanEqualTo20Euros()
        {
            //Arrange
            Payment p = paymentFaker.Generate();
            p.Amount = 15;
            _cheapPaymentGateway.Setup(a => a.ProcessPayment()).Returns(1);

            //Act
            _paymentService.ProcessPayment(p);

            //Assert
            _cheapPaymentGateway.Verify(a => a.ProcessPayment(), Times.AtLeastOnce);
        }

        [Fact]
        public void ProcessPayment_ShouldProcessUsingExpensivePaymentGateway_WhenExpensivePaymentGatewayIsAvailable()
        {
            //Arrange
            Payment p = paymentFaker.Generate();
            p.Amount = 30;

            _expensivePaymentGateway.Setup(a => a.IsAvailable()).Returns(true);
            _expensivePaymentGateway.Setup(a => a.ProcessPayment()).Returns(1);

            //Act
            _paymentService.ProcessPayment(p);

            //Assert
            _expensivePaymentGateway.Verify(async => async.ProcessPayment(), Times.Once);
        }

        [Fact]
        public void ProcessPayment_ShouldProcessUsingCheapPaymentGateway_WhenExpensivePaymentGatewayIsNotAvailable()
        {
            //Arrange
            Payment p = paymentFaker.Generate();
            p.Amount = 30;

            _expensivePaymentGateway.Setup(a => a.IsAvailable()).Returns(false);
            _cheapPaymentGateway.Setup(a => a.ProcessPayment()).Returns(1);

            //Act
            _paymentService.ProcessPayment(p);

            //Assert
            _cheapPaymentGateway.Verify(async => async.ProcessPayment(), Times.Once);
        }

        [Fact]
        public void ProcessPayment_ShouldProcessUsingPremiumPaymentGateway_WhenAmountGreaterThan500()
        {
            //Arrange
            Payment p = paymentFaker.Generate();
            p.Amount = 1000;

            _premiumPaymentGateway.Setup(a => a.ProcessPayment()).Returns(1);

            //Act
            _paymentService.ProcessPayment(p);

            //Assert
            _premiumPaymentGateway.Verify(async => async.ProcessPayment(), Times.Once);
        }

        [Fact]
        public void ProcessPayment_ShouldProcessUsingPremiumPaymentGatewayAndRetryUpto3TimesOnFailure_WhenAmountGreaterThan500()
        {
            //Arrange
            Payment p = paymentFaker.Generate();
            p.Amount = 1000;

            _premiumPaymentGateway.Setup(a => a.ProcessPayment()).Returns(0);

            //Act
            var ex = Record.Exception(() => _paymentService.ProcessPayment(p));

            //Assert
            Assert.Equal("Premium Payment Failed", ex.Message);
            _premiumPaymentGateway.Verify(async => async.ProcessPayment(), Times.Exactly(4));
        }

    }
}
