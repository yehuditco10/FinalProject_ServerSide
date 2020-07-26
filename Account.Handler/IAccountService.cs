using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Handler
{
    public interface IAccountService
    {
        Task<bool> idsCorrectness(Guid fromAccountId, Guid toAccountId);
        Task<bool> HasBalance(Guid fromAccountId, int amount);
        Task<bool> DoTransaction(Guid fromAccountId, Guid toAccountId, int amount);
    }
}
