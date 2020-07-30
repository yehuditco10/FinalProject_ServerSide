using Account.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Services.Interfaces
{
   public interface IVerificationRepository
    {
        Task<bool> VerifyEmail(EmailVerification verification);
        Task<EmailVerification> CreateEmailVerificationAsync(EmailVerification emailVerification);
        Task<EmailVerification> GetVerificationDetails(string email);
        Task<bool> UpdateVerificationCodeAsync(string email, int verificationCode);
    }
}
