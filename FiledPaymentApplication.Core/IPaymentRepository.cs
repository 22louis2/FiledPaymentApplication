using FiledPaymentApplication.Model;
using System.Threading.Tasks;

namespace FiledPaymentApplication.Core
{
    public interface IPaymentRepository
    {
        Task<bool> Add(Payment model);
    }
}
