using Account.Services.Models;
using System;
using System.Threading.Tasks;

namespace Account.Services.Interfaces
{
    public interface ITransferenceRepository
    {
       Task<bool> IsAccountExistsAsync(Guid fromAccountId);
       Task<int> GetBalance(Guid accountId);
        void DoTransaction(Transaction transaction);
    }
}