using System;
using System.ComponentModel.DataAnnotations;

namespace Account.Services.Models
{
   public class EmailVerification
    {
        [Required]
        public string Email { get; set; }
        public int VerificationCode { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
