using FiledPaymentApplication.Common;
using FiledPaymentApplication.Core;
using FiledPaymentApplication.Model;
using Moq;
using System;
using Xunit;

namespace FiledPaymentApplication.Test
{
    public class PaymentTest 
    {
        private readonly Mock<ICheapPaymentGateway> _cheapPayment;
        private readonly Mock<IExpensivePaymentGateway> _expensivePayment;
        private readonly Mock<IPremiumPaymentGateway> _premiumPayment;

        public PaymentTest()
        {
            _cheapPayment = new Mock<ICheapPaymentGateway>();
            _expensivePayment = new Mock<IExpensivePaymentGateway>();
            _premiumPayment = new Mock<IPremiumPaymentGateway>();
        }

        [Theory]
        [InlineData(20, "Louis Otu", "5334771104426541", "2/20/2056", "007")]
        [InlineData(10, "Jane Fowler", "5500000000000004", "2/20/2056", "346")]
        [InlineData(15, "Magic Mike", "4111111111111111 ", "2/20/2056", "377")]
        public void CheckCheapPaymentTestWorksAsExpected(decimal amount, string cardHolder, string creditCardNumber, string date, 
            string code)
        {
            var payment = new Payment
            {
                Amount = amount,
                CardHolder = cardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(date),
                SecurityCode = code
            };

            _cheapPayment.Setup(x => x.ProcessPayment(payment)).ReturnsAsync(true);

            Assert.True(_cheapPayment.Object.ProcessPayment(payment).Result);
        }

        [Theory]
        [InlineData(22, "Louis Otu", "5334771104426541", "2/20/2056", "007")]
        [InlineData(230, "Jane Fowler", "5500000000000004", "2/20/2056", "346")]
        [InlineData(415, "Magic Mike", "4111111111111111 ", "2/20/2056", "377")]
        public void CheckExpensivePaymentTestWorksAsExpected(decimal amount, string cardHolder, string creditCardNumber, string date, 
            string code)
        {
            var payment = new Payment 
            {
                Amount = amount,
                CardHolder = cardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(date),
                SecurityCode = code
            };

            _expensivePayment.Setup(x => x.ProcessPayment(payment)).ReturnsAsync(true);

            Assert.True(_expensivePayment.Object.ProcessPayment(payment).Result);
        }

        [Theory]
        [InlineData(1230, "Louis Otu", "5334771104426541", "2/20/2056", "007")]
        [InlineData(6700, "Jane Fowler", "5500000000000004", "2/20/2056", "346")]
        [InlineData(10000, "Magic Mike", "4111111111111111 ", "2/20/2056", "377")]
        public void CheckPremiumPaymentTestWorksAsExpected(decimal amount, string cardHolder, string creditCardNumber, string date, 
            string code)
        { 
            var payment = new Payment
            {
                Amount = amount,
                CardHolder = cardHolder,
                CreditCardNumber = creditCardNumber,
                ExpirationDate = DateTime.Parse(date),
                SecurityCode = code
            };

            _premiumPayment.Setup(x => x.ProcessPayment(payment)).ReturnsAsync(true);

            Assert.True(_premiumPayment.Object.ProcessPayment(payment).Result);
        }

        [Fact]
        public void CheckIfCreditCardIsValid()
        {
            string cardNumber = "5334771104426541";
            var result = Utils.IsCardNumberValid(cardNumber);
            Assert.True(result);
        }

        [Fact]
        public void CheckIfCreditCardIsInvalid()
        {
            string cardNumber = "4111111111111223";
            var result = Utils.IsCardNumberValid(cardNumber);
            Assert.False(result);
        }

        [Fact]
        public void CheckIfDateInThePastIsValid()
        {
            DateTime date = DateTime.Now.AddDays(1);
            var result = Utils.IsDateInThePast(date);
            Assert.True(result);
        }

        [Fact]
        public void CheckIfDateInThePastIsInValid()
        {
            DateTime date = DateTime.Parse("2/10/2021");
            var result = Utils.IsDateInThePast(date);
            Assert.False(result);
        }

        [Fact]
        public void CheckIfDateIsInCorrectFormatAndNotInThePast()
        {
            string date = "02/02/2079";
            var result = Utils.DateFormatValidator(date);
            Assert.True(result);
        }

        [Fact]
        public void CheckIfDateIsNotInCorrectFormat()
        {
            string date = "2/10/2021";
            var result = Utils.DateFormatValidator(date);
            Assert.False(result);
        }

        [Fact]
        public void CheckIfDateIsInCorrectFormatAndInThePast()
        {
            string date = "02/02/2020";
            var result = Utils.DateFormatValidator(date);
            Assert.False(result);
        }
    }
}
