using Account.Services.Exceptions;
using Account.Services.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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
        private string ToHash(string password)
        {
            //byte[] hashPassword;
            //new RNGCryptoServiceProvider().GetBytes(hashPassword = new byte[16]);
            //var pbkdf2 = new Rfc2898DeriveBytes(password, hashPassword, 100000);
            //byte[] hash = pbkdf2.GetBytes(20);
            //byte[] hashBytes = new byte[36];
            //Array.Copy(hashPassword, 0, hashBytes, 0, 16);
            //Array.Copy(hash, 0, hashBytes, 16, 20);
            //return Convert.ToBase64String(hashBytes);
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        public async Task<bool> CreateAsync(Customer customer)
        {
            var isExsits = _accountRepository.IsEmailExistsAsync(customer.Email);
            if (isExsits.Result == false)
            {
                //customer.Password = ToHash(customer.Password);
                var CreateAccountSucceded = await _accountRepository.CreateAccountAsync(customer);
                if (CreateAccountSucceded != -1)
                {
                    return true;
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

            // password = ToHash(password);
            Customer customer = await _accountRepository.GetCustomerAsync(email, password);
            return await _accountRepository.GetAccountIdByCustomerIdAsync(customer.Id);
        }


    }
}
