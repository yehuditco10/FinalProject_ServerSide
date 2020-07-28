using System;
using System.Linq;

namespace Account.Services.Models.Pagination
{
    public class QueryParameters
    {
        public int maxPageCount;
        public int Page { get; set; }
        private int _pageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        }
        public string Query { get; set; }
        public string OrderBy { get; set; }
        public Guid AccountId { get; set; }
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

