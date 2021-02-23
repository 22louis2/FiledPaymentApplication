namespace FiledPaymentApplication.Core
{
    public class PremiumPaymentService : CheapPaymentService, IPremiumPaymentGateway
    {
        public PremiumPaymentService(ITransactionRepository transactionRepository, IPaymentRepository paymentRepository)
            : base(transactionRepository, paymentRepository) { }
    }
}
