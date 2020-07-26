using NServiceBus;
using System;
using Transaction.Services.Models;

namespace Transaction.Handler
{
    public class TransactionPolicyData : ContainSagaData
    {
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public eStatus Status { get; set; }
        public string FailureReason { get; set; }
    }
}