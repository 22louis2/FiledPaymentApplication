using FiledPaymentApplication.Model;
using System.Threading.Tasks;

namespace FiledPaymentApplication.Core
{
    public interface ICheapPaymentGateway
    {
        Task<bool> ProcessPayment(Payment model);
        Task<bool> ProcessTransaction(string id);
    }
}
