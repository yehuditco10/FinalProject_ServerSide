using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace Account.Api.DTO
{
    public class Login
    {
        [Email]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
