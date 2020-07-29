using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Services.Models.Pagination
{
   public class Filter
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string IsCredit { get; set; }
    }
}
