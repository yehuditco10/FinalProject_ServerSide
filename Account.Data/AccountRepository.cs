using Account.Data.Entities;
using Account.Services;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountContext _accountContext;
        private readonly IMapper _mapper;

        public AccountRepository(AccountContext accountContext,
            IMapper mapper)
        {
            _accountContext = accountContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateAccountAsync(string email)
        {
            try
            {
                var customer = await _accountContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
                if (customer != null)
                {
                    var account = new Entities.Account()
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = customer.Id,
                        Opendate = DateTime.Today,
                        Balance = 1000
                    };
                    await _accountContext.Accounts.AddAsync(account);
                    var createSucceeded = await _accountContext.SaveChangesAsync();
                    return createSucceeded != -1 ? true : false;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + " create account failed");
            }
        }

        public async Task<int> CreateCustomerAsync(Services.Models.Customer customer)
        {
            try
            {
                Entities.Customer newCustomer = _mapper.Map<Entities.Customer>(customer);
                await _accountContext.Customers.AddAsync(newCustomer);
                return await _accountContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            Entities.Customer customer = await _accountContext.Customers.FirstOrDefaultAsync(
                c => c.Email == email);
            if (customer == null)
            {
                return false;
            }
            return true;
        }
        public async Task<Services.Models.Customer> GetCustomerAsync(string email, string password)
        {
            try
            {
                Entities.Customer customer = await _accountContext.Customers
              .FirstOrDefaultAsync(c => c.Email == email && c.Password == password);
                if (customer != null)
                    return _mapper.Map<Services.Models.Customer>(customer);
                return null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<Services.Models.Account> GetAccountAsync(Guid accountId)
        {
            try
            {
                var account = await _accountContext.Accounts
                   .Include(c => c.Customer)
                     .FirstOrDefaultAsync(a => a.Id == accountId);
                if (account != null)
                {
                    return _mapper.Map<Services.Models.Account>(account);
                }
                return null;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message+" get account failed");
            }
            throw new NotImplementedException();
        }

        public async Task<Guid> GetAccountIdByCustomerIdAsync(Guid customerId)
        {
            try
            {
                var account = await _accountContext.Accounts
                   .FirstOrDefaultAsync(a => a.CustomerId == customerId);
                if (account != null)
                {
                    return account.Id;
                }
                return Guid.Empty;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            throw new NotImplementedException();
        }
    }
}
