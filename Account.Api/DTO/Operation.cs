using System;

namespace Account.Api.DTO
{
    public class Operation
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public bool IsCredit { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public DateTime Date { get; set; }
    }
}
