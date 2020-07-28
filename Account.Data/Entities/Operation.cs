using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Entities
{
   public class Operation
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
        public Guid TransactionID { get; set; }
        public bool IsCredit { get; set; }
        public int TransactionAmount { get; set; }
        public int Balance { get; set; }
        public DateTime OperationTime { get; set; }
    }
}
