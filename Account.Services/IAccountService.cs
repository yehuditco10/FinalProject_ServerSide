using Account.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Services
{
    public interface IAccountService
    {
        Task<bool> CreateAsync(Customer customer);
        Task<Guid> LoginAsync(string email, string password);
        Task<Models.Account> GetAccountAsync(Guid accountId);
        int GenerateRandomNo(int min, int max);
        //Task<bool> DoTransaction(Transaction transaction);
        //Task<bool> HasBalance(Guid fromAccountId, int amount);
        //Task<bool> AccountIdCorrectness(Guid fromAccountId);
        Task CreateTransaction(Transaction transaction);
    }
}
