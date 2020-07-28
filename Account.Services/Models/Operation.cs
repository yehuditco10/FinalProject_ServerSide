using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Services.Models
{
    public class Operation
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public bool IsCredit { get; set; }
        public int Amount { get; set; }
        public int Balance { get; set; }
        public DateTime Date { get; set; }
        public Link Link { get; set; }
    }
    public class Link
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }

        public Link(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
