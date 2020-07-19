using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Api.DTO;
using Account.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountService,
               IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<bool>> CreateAccountAsync(Customer customerDTO)
        {
            var customer = _mapper.Map<Services.Models.Customer>(customerDTO);
            return await _accountService.CreateAsync(customer);
        }
        [HttpGet]
        public async Task<IActionResult> LoginAsync(Login loginCustomer)
        {
            Guid accountId = await _accountService.LoginAsync(loginCustomer.Email, loginCustomer.Password);
            if (accountId != Guid.Empty)
            {
                return Ok(accountId);
            }
            return Unauthorized();
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetInfoAsync(Guid accountId)
        {
            Services.Models.Account account = await _accountService.GetAccountAsync(accountId);
            if (account != null)
            {
                AccountInfo accountInfo = new AccountInfo()
                {
                    FirstName = account.Customer.FirstName,
                    LastName = account.Customer.LastName,
                    Balance = account.Balance,
                    Opendate = account.Opendate
                };
                return Ok(accountInfo);
            }
            return NotFound();
        }
    }
}