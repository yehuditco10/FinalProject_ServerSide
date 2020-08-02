using Account.Services.Models;
using System.Threading.Tasks;

namespace Account.Services.Interfaces
{
   public interface IVerificationRepository
    {
        Task<bool> VerifyEmail(EmailVerification verification);
        Task<int> CreateEmailVerificationAsync(EmailVerification emailVerification);
        Task<EmailVerification> GetVerificationDetails(string email);
        Task<bool> UpdateVerificationCodeAsync(EmailVerification emailVerification);
    }
}
