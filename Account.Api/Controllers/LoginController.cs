using Account.Api.DTO;
using Account.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Account.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class LoginController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IVerificationService _verificationService;
        private readonly IMapper _mapper;
        public LoginController(IAccountService accountService,
            IVerificationService verificationService,
               IMapper mapper)
        {
            _accountService = accountService;
            _verificationService = verificationService;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<ActionResult<bool>> CreateAccountAsync(Customer customerDTO)
        {
            var emailVerification = new Services.Models.EmailVerification()
            {
                Email = customerDTO.Email,
                VerificationCode = Int32.Parse(customerDTO.VerificationCode)
            };
            var ok = await _verificationService.VerifyEmail(emailVerification);
            if (!ok)
            {
                return BadRequest("The code not match");
            }
            var customer = _mapper.Map<Services.Models.Customer>(customerDTO);
            return await _accountService.CreateAsync(customer);
        }
        [HttpGet]
        public async Task<IActionResult> LoginAsync([FromQuery] Login loginCustomer)
        {
            Guid accountId = await _accountService.LoginAsync(loginCustomer.Email, loginCustomer.Password);
            if (accountId != Guid.Empty)
            {
                return Ok(accountId);
            }
            return Unauthorized();
        }
    }
}