using Account.Services.Models;
using System.Threading.Tasks;

namespace Account.Services.Interfaces
{
    public interface ITransactionService
    {
        Task CreateTransaction(Transaction transaction);
    }
}
