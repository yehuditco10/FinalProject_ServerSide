using System;
using System.Collections.Generic;
using System.Text;

namespace Transaction.Services.Models
{
  public  class TransactionStatus
    {
        public Guid TransactionId { get; set; }
        public bool isSucceeded { get; set; }
        public string FailureReason { get; set; }
    }
}
