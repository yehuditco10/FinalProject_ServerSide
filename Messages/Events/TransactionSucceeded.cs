﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Messages.Events
{
    public class TransactionSucceeded
    {
        public Guid TransactionId { get; set; }
        public Guid FromAccountId { get; set; }
        public Guid ToAccountId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
