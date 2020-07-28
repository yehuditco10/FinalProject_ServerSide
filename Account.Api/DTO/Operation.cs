using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Api.DTO
{
    public class Operation
    {
        public bool IsCredit { get; set; }
        public Guid AccountId { get; set; }
        public int Amount { get; set; }
        public Guid Id { get; set; }
        public int Balance { get; set; }
        public DateTime Date { get; set; }

    }
}
