using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
   public class TransactionCreated:IEvent
    {
        public Guid TransactionId { get; set; }
        public bool IsSucceeded { get; set; }
        public string FailureReason { get; set; }
    }
}
