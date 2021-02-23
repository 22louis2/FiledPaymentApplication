using FiledPaymentApplication.Model;
using System.Threading.Tasks;

namespace FiledPaymentApplication.Core
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetByPaymentId(string id);
        Task<bool> Add(Transaction model);
    }
}
