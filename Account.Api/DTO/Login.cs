using System.ComponentModel.DataAnnotations;

namespace Account.Api.DTO
{
    public class Login
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
