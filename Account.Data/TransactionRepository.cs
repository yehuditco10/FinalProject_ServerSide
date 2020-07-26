using Account.Services.Models;
using Account.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Data
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AccountContext _accountContext;
        public TransactionRepository(AccountContext accountContext)
        {
            _accountContext = accountContext;
        }
        public async Task<bool> IsAccountExistsAsync(Guid accountId)
        {
           var exist= await _accountContext.Accounts.FirstOrDefaultAsync(i => i.Id == accountId);
            return exist.Id != null;
        } 
        public async Task<int> GetBalance(Guid accountId)
        {
           var account= await _accountContext.Accounts.FirstOrDefaultAsync(i => i.Id == accountId);
            return account.Balance;
        }
        public async void DoTransaction(Transaction transaction)
        {
            try
            {
                var from =await _accountContext.Accounts.FirstOrDefaultAsync(account => account.Id == transaction.FromAccountId);
                var to = await _accountContext.Accounts.FirstOrDefaultAsync(account => account.Id == transaction.ToAccountId);
                from.Balance -= transaction.Amount;
                to.Balance += transaction.Amount;
                await  _accountContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
