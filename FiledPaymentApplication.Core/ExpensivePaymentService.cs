namespace FiledPaymentApplication.Core
{
    public class ExpensivePaymentService : CheapPaymentService, IExpensivePaymentGateway
    {
        public ExpensivePaymentService(ITransactionRepository transactionRepository, IPaymentRepository paymentRepository)
            : base(transactionRepository, paymentRepository) { }
    }
}
