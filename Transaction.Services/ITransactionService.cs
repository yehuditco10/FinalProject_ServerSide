using System;
using System.Threading.Tasks;
using Transaction.Services.Models;

namespace Transaction.Services
{
   public interface ITransactionService
    {
       Task<bool> DoTransactionAsync(Models.Transaction transaction);
        Task UpdateStatus(TransactionStatus transactionStatus);
        Task<Models.Transaction> GetTransactionDetailes(Guid transactionId);
    }
}
