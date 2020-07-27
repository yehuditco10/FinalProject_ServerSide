using Account.Services.Models;
using Messages.Events;
using System.Threading.Tasks;

namespace Account.Services.Interfaces
{
    public interface ITransferenceService
    {
        Task<TransactionCreated> CreateTransaction(Transaction transaction);
    }
}
