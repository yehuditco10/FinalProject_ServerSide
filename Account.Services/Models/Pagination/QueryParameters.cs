using System;
using System.Linq;

namespace Account.Services.Models.Pagination
{
    public class QueryParameters
    {
        public int maxPageCount;
        public int Page { get; set; }
        public int PageCount { get; set; }
        public string Query { get; set; }
        public string OrderBy { get; set; }
        public Guid AccountId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Type { get; set; }
        public QueryParameters()
        {
            Query = "operationDate";
        }
        public bool HasPrevious()
        {
            return (Page > 1);
        }

        public bool HasNext(int totalCount)
        {
            return Page < (int)GetTotalPages(totalCount);
        }

        public int GetTotalPages(int totalCount)
        {
            if (PageCount == 0)
                return 0;
            return (int)Math.Ceiling(totalCount / (double)PageCount);
        }

        public bool HasQuery()
        {
            return !String.IsNullOrEmpty(Query);
        }

        public bool IsDescending()
        {
            if (!String.IsNullOrEmpty(OrderBy))
            {
                return OrderBy.
                    Split(' ').Last().ToLowerInvariant().StartsWith("desc");
            }
            return false;
        }
    }
}

