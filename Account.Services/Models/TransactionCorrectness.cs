﻿
namespace Account.Services.Models
{
   public class TransactionCorrectness
    {
        public bool IsValid { get; set; }
        public string Reason { get; set; }
        public TransactionCorrectness(bool IsValid)
        {
            this.IsValid = IsValid;
        }
    }
}
