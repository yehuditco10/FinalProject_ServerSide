using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Services.Models.Pagination
{
   public class PaginationMetadata
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
