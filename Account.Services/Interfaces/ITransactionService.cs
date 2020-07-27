using Account.Services.Models;
using Messages.Events;
using System.Threading.Tasks;

namespace Account.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionCreated> CreateTransaction(Transaction transaction);
    }
}
