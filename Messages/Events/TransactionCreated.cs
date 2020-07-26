using System;

namespace Messages.Events
{
   public class TransactionCreated
    {
        public Guid TransactionId { get; set; }
        public bool IsSucceeded { get; set; }
        public string FailureReason { get; set; }
    }
}
