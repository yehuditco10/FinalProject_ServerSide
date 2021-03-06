﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Data.Entities
{
    public class EmailVerification
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int VerificationCode { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
