using Account.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository )
        {
            _accountRepository = accountRepository;
        }
        public async Task<bool> CreateAsync(Customer customer)
        {
                var isExsits = _accountRepository.IsEmailExistsAsync(customer.Email);
                if (isExsits.Result == false)
                {
                    var CreateAccountSucceded = await _accountRepository.CreateCustomerAsync(customer);
                    if (CreateAccountSucceded !=-1)
                    {
                        return await _accountRepository.CreateAccountAsync(customer.Email);
                    }
                }
                return false;
        }

        public async Task<Models.Account> GetAccountAsync(Guid accountId)
        {
            return await _accountRepository.GetAccountAsync(accountId);
        }

        public async Task<Guid> LoginAsync(string email, string password)
        {
            Customer customer = await _accountRepository.GetCustomerAsync(email, password);
            if (customer != null)
            {
                 return await _accountRepository.GetAccountByCustomerIdAsync(customer.Id);
            }
            return Guid.Empty;
        }
    }
}
