using System;
using System.Threading.Tasks;
using Transaction.Services.Models;

namespace Transaction.Services
{
    public interface ITransactionRepository
    {
        Task<bool> AddTransactionToDB(Models.Transaction transaction);
        Task<int> UpdateStatus(Guid transactionId, eStatus status);
        Task UpdateStatus(TransactionStatus transactionStatus);
    }
}
