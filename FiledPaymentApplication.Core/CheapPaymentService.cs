using FiledPaymentApplication.Model;
using System;
using System.Threading.Tasks;

namespace FiledPaymentApplication.Core
{
    public class CheapPaymentService : ICheapPaymentGateway
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPaymentRepository _paymentRepository;

        public CheapPaymentService(ITransactionRepository transactionRepository, IPaymentRepository paymentRepository)
        {
            _transactionRepository = transactionRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<bool> ProcessPayment(Payment model)
        {
            model.Id = Guid.NewGuid().ToString();
            await _paymentRepository.Add(model);
            return await ProcessTransaction(model.Id);
        }

        public async Task<bool> ProcessTransaction(string id)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                PaymentId = id
            };
            return await _transactionRepository.Add(transaction);
        }
    }
}
