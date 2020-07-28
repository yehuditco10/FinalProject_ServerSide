using System;
using System.ComponentModel.DataAnnotations;

namespace Transaction.Api.DTO
{
    public class Transaction
    {
        [Required]
        public Guid FromAccountId { get; set; }
        [Required]
        public Guid ToAccountId { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
