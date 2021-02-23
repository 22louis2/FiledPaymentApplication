using FiledPaymentApplication.Data;
using FiledPaymentApplication.Model;
using System.Threading.Tasks;

namespace FiledPaymentApplication.Core
{
    public class PaymentRepository : IPaymentRepository
    {
        private AppDbContext Context { get; set; }
        public PaymentRepository(AppDbContext context)
        {
            Context = context;
        }

        public async Task<bool> Add(Payment model)
        {
            await Context.Payments.AddAsync(model);
            return await Context.SaveChangesAsync() > 0;
        }
    }
}
