using System;

namespace Transaction.Api.DTO
{
    public class Transaction
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public int Amount { get; set; }
    }
}
