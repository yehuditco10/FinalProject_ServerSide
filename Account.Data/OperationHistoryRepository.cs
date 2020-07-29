using Account.Services.Interfaces;
using Account.Services.Models;
using Account.Services.Models.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Account.Data
{
    public class OperationHistoryRepository : IOperationHistoryRepository
    {
        private readonly AccountContext _accountContext;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;

        public OperationHistoryRepository(AccountContext accountContext, IMapper mapper
            //,IUrlHelper iURLHelper
            )
        {
            _accountContext = accountContext;
            _mapper = mapper;
           // _urlHelper = iURLHelper;
        }
        public int GetCountPerAccount(Guid accountId)
        {
            return _accountContext.Operations.Where(a => a.AccountId == accountId).Count();
        }

        public List<Services.Models.Operation> GetOperationsOrdered(QueryParameters queryParameters)
        {
            var totalCount = GetCountPerAccount(queryParameters.AccountId);
            IQueryable<Entities.Operation> _allItems;
            if (totalCount == 0)
            {
                //to do
                throw new Exception("no content");
            }

            if (queryParameters.HasQuery())
            {
               // IComparer<string> descending = (IComparer<string>)(queryParameters.IsDescending() == true ? "Desc" : "").ToList();
                var f = queryParameters.Query.ToLowerInvariant();
                _allItems = _accountContext.Operations.Where
            //(x => x.OperationTime.ToString().Contains(queryParameters.Query.ToLowerInvariant())&& // וגם צריך להוסיף כזה משפט לכל פרמטר שרוצים לסדר לפיו.לא עושה הפוך
            (x => x.AccountId == queryParameters.AccountId)
                .OrderBy(key => queryParameters.OrderBy);
            }
            else
            {
                _allItems = _accountContext.Operations.Where(x => x.AccountId == queryParameters.AccountId).OrderBy(key => queryParameters.OrderBy);
            }
          //  var links = CreateLinksForCollection(queryParameters, totalCount);
            var query = _allItems
        .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                            .Take(queryParameters.PageCount);
            return _mapper.Map<List<Services.Models.Operation>>(query);
        }
        //private List<Link> CreateLinksForCollection(QueryParameters queryParameters, int totalCount)
        //{
        //    var links = new List<Link>
        //    {
        //        new Link(_urlHelper.Link(nameof(Add), null), "create", "POST"),

        //        // self 
        //        new Link(_urlHelper.Link(nameof(GetOperationsOrdered), new
        //        {
        //            pagecount = queryParameters.PageCount,
        //            page = queryParameters.Page,
        //            orderby = queryParameters.OrderBy
        //        }), "self", "GET"),

        //        new Link(_urlHelper.Link(nameof(GetOperationsOrdered), new
        //        {
        //            pagecount = queryParameters.PageCount,
        //            page = 1,
        //            orderby = queryParameters.OrderBy
        //        }), "first", "GET"),

        //        new Link(_urlHelper.Link(nameof(GetOperationsOrdered), new
        //        {
        //            pagecount = queryParameters.PageCount,
        //            page = queryParameters.GetTotalPages(totalCount),
        //            orderby = queryParameters.OrderBy
        //        }), "last", "GET")
        //    };

        //    if (queryParameters.HasNext(totalCount))
        //    {
        //        links.Add(new Link(_urlHelper.Link(nameof(GetOperationsOrdered), new
        //        {
        //            pagecount = queryParameters.PageCount,
        //            page = queryParameters.Page + 1,
        //            orderby = queryParameters.OrderBy
        //        }), "next", "GET"));
        //    }

        //    if (queryParameters.HasPrevious())
        //    {
        //        links.Add(new Link(_urlHelper.Link(nameof(GetOperationsOrdered), new
        //        {
        //            pagecount = queryParameters.PageCount,
        //            page = queryParameters.Page - 1,
        //            orderby = queryParameters.OrderBy
        //        }), "previous", "GET"));
        //    }

        //    return links;
        //}
        public void Add(Entities.Operation item)
        {
            _accountContext.Operations.Add(item);
        }
        public async Task CreateOperation(Guid accountId, int amount, string type, Guid transactionId)
        {
            Entities.Account account = await _accountContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            Entities.Operation operation = new Entities.Operation()
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                IsCredit = (type == "credit" ? true : false),
                Amount = amount,
                Balance = account.Balance,
                OperationTime = DateTime.Now,
                TransactionID = transactionId
            };
            await _accountContext.AddAsync(operation);
            await _accountContext.SaveChangesAsync();
        }
    }
}
