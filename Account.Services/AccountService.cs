using Account.Services.Exceptions;
using Account.Services.Models;
using Account.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public async Task<bool> CreateAsync(Customer customer)
        {
            var isExsits = _accountRepository.IsEmailExistsAsync(customer.Email);
            if (isExsits.Result == false)
            {
                customer.PasswordSalt = Hashing.GetSalt();
                customer.PasswordHash = Hashing.GenerateHash(customer.Password, customer.PasswordSalt);
                var isCreated = await _accountRepository.CreateAccountAsync(customer);
                if (isCreated == false)
                    throw new Exception("Creation failed");
                return true;
            }
            return false;
        }
       
        public int GenerateRandomNo(int min, int max)
        {
            Random _rdm = new Random();
            return _rdm.Next(min, max);
        }
        public async Task<Models.Account> GetAccountAsync(Guid accountId)
        {
            return await _accountRepository.GetAccountAsync(accountId);
        }
        public async Task<Guid> LoginAsync(string email, string password)
        {
            Customer customer = await _accountRepository.GetCustomerAsync(email);
            bool isValid = VerifyHashedPassword(customer.PasswordHash, customer.PasswordSalt, password);
            if (isValid == false)
                throw new LoginFailedException("Your password is not valid");
            return await _accountRepository.GetAccountIdByCustomerIdAsync(customer.Id);
        }
        private bool VerifyHashedPassword(string customerPassword, string customerSaltPassword, string passwordFromUser)
        {
            if (Hashing.AreEqual(passwordFromUser, customerPassword, customerSaltPassword))
                return true;
            return false;
        }
    }
}
