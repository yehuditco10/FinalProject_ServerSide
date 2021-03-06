﻿using Account.Services.Models;
using System;
using System.Threading.Tasks;

namespace Account.Services.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> IsEmailExistsAsync(string email);
        Task<bool> CreateAccountAsync(Customer customer);
        Task<Customer> GetCustomerAsync(string email);
        Task<Models.Account> GetAccountAsync(Guid accountId);
        Task<Guid> GetAccountIdByCustomerIdAsync(Guid customerId);
    }
}
