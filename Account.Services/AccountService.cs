using Account.Services.Exceptions;
using Account.Services.Models;
using Messages.Events;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMessageSession _messageSession;

        public AccountService(IAccountRepository accountRepository,
            IMessageSession messageSession
           )
        {
            _accountRepository = accountRepository;
            _messageSession = messageSession;
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

        public async Task CreateTransaction(Transaction transaction)
        {
            TransactionCorrectness transactionCorrectness = await AccountIdsCorrectness(transaction.FromAccountId, transaction.ToAccountId);
            if (transactionCorrectness.IsValid == true)
            {
                transactionCorrectness = await HasBalance(transaction.FromAccountId, transaction.Amount);
                if (transactionCorrectness.IsValid == true)
                {
                    bool isSucceeded = await DoTransaction(transaction);
                }
            }
            TransactionCreated transactionCreated = new TransactionCreated()
            {
                TransactionId = transaction.TransactionId,
                IsSucceeded=transactionCorrectness.IsValid,
                FailureReason =transactionCorrectness.Reason
            };
            await _messageSession.Publish(transactionCreated);
        }

        private Task<TransactionCorrectness> AccountIdsCorrectness(Guid fromAccountId, Guid toAccountId)
        {
            throw new NotImplementedException();
        }

        private Task<TransactionCorrectness> HasBalance(Guid fromAccountId, int amount)
        {
            throw new NotImplementedException();
        }

        private Task<bool> DoTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
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
