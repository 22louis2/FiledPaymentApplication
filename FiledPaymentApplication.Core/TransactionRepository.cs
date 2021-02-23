using FiledPaymentApplication.Data;
using FiledPaymentApplication.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FiledPaymentApplication.Core
{
    public class TransactionRepository : ITransactionRepository
    {
        private AppDbContext Context { get; set; }
        public TransactionRepository(AppDbContext context)
        {
            Context = context;
        }

        public async Task<Transaction> GetByPaymentId(string id)
        {
            return await Context.Transactions.Include(x => x.Payment).FirstOrDefaultAsync(x => x.PaymentId == id);
        }

        public async Task<bool> Add(Transaction model)
        {
            await Context.Transactions.AddAsync(model);
            return await Context.SaveChangesAsync() > 0; 
        }
    }
}
