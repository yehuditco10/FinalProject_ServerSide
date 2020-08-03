
using Microsoft.Extensions.Configuration;
using System;

namespace Account.Api.DTO
{
    public class QueryParameters
    {
        //public QueryParameters(IConfiguration configuration)
        //{
        //    maxPageCount = configuration.GetValue<int>("MaxPageCount");
        //    if (maxPageCount == 0) maxPageCount = 50;//רק כדי שלא יעוף
        //    _pageCount = maxPageCount;
        //}
        public Guid AccountId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Type { get; set; }

        // private int maxPageCount = 50;
        public int Page { get; set; } = 1;
        private int _pageCount;
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > 50) ? 50 : value; }
        }
        public string Query { get; set; }
        public string OrderBy { get; set; }
    }
}
