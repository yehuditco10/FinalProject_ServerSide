﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transaction.Api.DTO
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public eStatus Status { get; set; }
        public string FailureReason { get; set; }
    }

    public enum eStatus
    {
        processing = 1, successed, failed
    }
}
