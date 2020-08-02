using System;
using System.Threading.Tasks;
using Account.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    public class VerificationController : ControllerBase
    {
        private readonly IVerificationService _verificationService;
        private readonly IMapper _mapper;
        public VerificationController(IVerificationService verificationService,
               IMapper mapper)
        {
            _verificationService = verificationService;
            _mapper = mapper;
        }
        [HttpGet("resend")]
        public async Task<ActionResult> ReSendVerificationCodeAsync(string email)
        {
            await _verificationService.ReSendVerificationCodeAsync(email);
            return Ok();
        }
        [HttpGet("verification")]
        public async Task<ActionResult<bool>> GenerateEmailVerificationAsync(string email)
        {
            if (String.IsNullOrEmpty(email) || !(email.Contains("@")))
                return BadRequest("not valid email address");
            await _verificationService.SendVerificationCodeAsync(email);
            return Ok();
        }
    }
}