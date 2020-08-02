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
        Task SendVerificationCodeAsync(string email);
        Task ReSendVerificationCodeAsync(string email);
    }
}
