using Account.Data.Entities;
using Account.Data.Exceptions;
using Account.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Account.Data
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly AccountContext _accountContext;
        private readonly IMapper _mapper;

        public VerificationRepository(AccountContext accountContext,
            IMapper mapper)
        {
            _accountContext = accountContext;
            _mapper = mapper;
        }
        public async Task<int> CreateEmailVerificationAsync(Services.Models.EmailVerification emailVerificationModel)
        {
                Entities.EmailVerification emailVerification = _mapper.Map<Entities.EmailVerification>(emailVerificationModel);
                _accountContext.EmailVerificationS.Add(emailVerification);
                return await _accountContext.SaveChangesAsync();                          
        }
        public async Task<bool> VerifyEmail(Services.Models.EmailVerification verification)
        {
            var emailVerification = await _accountContext.EmailVerificationS.FirstOrDefaultAsync(c => c.Email == verification.Email);
                if (emailVerification == null)
                    throw new EmailVerificationException("Email not found");
                if (emailVerification.VerificationCode != verification.VerificationCode)
                    throw new EmailVerificationException("The verification code is wrong");
                if (emailVerification.ExpirationTime < DateTime.Now)
                    throw new EmailVerificationException("The expiration time has expired");
                return true;  
        }
        public async Task<Services.Models.EmailVerification> GetVerificationDetails(string email)
        {
           Entities.EmailVerification emailVerification = await _accountContext.EmailVerificationS.FirstOrDefaultAsync(
                              e => e.Email == email);
                if (emailVerification == null)
                    throw new AccountNotFoundException("we didn't find this email");//which exception
                return _mapper.Map<Services.Models.EmailVerification>(emailVerification);
        }
        public async Task<bool> UpdateVerificationCodeAsync(Services.Models.EmailVerification emailVerification)
        {
            EmailVerification oldEmailVerification = await _accountContext.EmailVerificationS.FirstOrDefaultAsync(
                               e => e.Email == emailVerification.Email);
                if (emailVerification == null)
                    throw new AccountNotFoundException("we didn't find this email");//which exception
                oldEmailVerification.VerificationCode = emailVerification.VerificationCode;
                oldEmailVerification.ExpirationTime = emailVerification.ExpirationTime;
               return await _accountContext.SaveChangesAsync()>0;                
        }
    }
}
