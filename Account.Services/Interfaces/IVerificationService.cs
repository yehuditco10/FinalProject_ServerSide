using Account.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Services.Interfaces
{
   public interface IVerificationService
    {
        Task<bool> VerifyEmail(EmailVerification verificationModel);
        Task SendVerificationCodeAsync(EmailVerification emailVerification);
        Task ReSendVerificationCodeAsync(string email);
    }
}
