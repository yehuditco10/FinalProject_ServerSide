using System.ComponentModel.DataAnnotations;

namespace Account.Api.DTO
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
