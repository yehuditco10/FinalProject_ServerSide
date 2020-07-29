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
    public class OperationRepository : IOperationRepository
    {
        private readonly AccountContext _accountContext;
        private readonly IMapper _mapper;
        //private readonly IUrlHelper _urlHelper;

        public OperationRepository(AccountContext accountContext, IMapper mapper
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

        public List<Operation> GetOperationsOrdered(QueryParameters queryParameters)
        {
            var totalCount = GetCountPerAccount(queryParameters.AccountId);
            IQueryable<Entities.Operation> _allItems;
            if (totalCount == 0)
            {
                //to do
                throw new Exception("no content");
            }

                _allItems = GetAllItemsWithSortingAndFiltering(queryParameters);
            if(_allItems.Count()==0)
            { throw new Exception("no data found for this filtering"); }
                //  var links = CreateLinksForCollection(queryParameters, totalCount);
                var query = _allItems
            .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                                .Take(queryParameters.PageCount);
                return _mapper.Map<List<Services.Models.Operation>>(query);
            }
        private IQueryable<Entities.Operation> GetAllItemsWithSortingAndFiltering(QueryParameters queryParameters)
        {

            var descending = queryParameters.IsDescending() == true ? "Desc" : "";
           var _allItems = _accountContext.Operations.Where
             (x => x.AccountId == queryParameters.AccountId)
            .OrderBy(key => queryParameters.OrderBy);
            return _allItems;
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
        public async Task CreateOperation(Operation operation)
        {
            Entities.Operation newOperation = _mapper.Map<Entities.Operation>(operation);
            Entities.Account account = await _accountContext.Accounts.FirstOrDefaultAsync(a => a.Id == operation.AccountId);
            newOperation.Id = Guid.NewGuid();
            newOperation.Balance = account.Balance;
            newOperation.OperationTime = DateTime.Now;
             _accountContext.Add(newOperation);
            await _accountContext.SaveChangesAsync();
        }




        public IQueryable<Entities.Operation> IsCredit(QueryParameters queryParameters, bool operationType)
        {
            return _accountContext.Operations.Where(t => t.IsCredit == operationType)
                                .OrderBy(queryParameters.OrderBy, queryParameters.IsDescending());
        }
        public IQueryable<Entities.Operation> ByTime(QueryParameters queryParameters, DateTime fromDate, DateTime untilDate)
        {
            return _accountContext.Operations.Where(t => t.OperationTime >= fromDate && t.OperationTime <=untilDate)
                                .OrderBy(queryParameters.OrderBy, queryParameters.IsDescending());
        }
        //public IQueryable<Operation> Filter(QueryParameters queryParameters, Filter filter)
        //{
        //    DateTime emptyDate = new DateTime();
        //    if (filter.FromDate == emptyDate && filter.ToDate == emptyDate
        //        && filter.OperationType != default)
        //        return IsCredit(queryParameters, filter.OperationType);
        //    if (filter.FromDate != emptyDate && filter.ToDate == emptyDate)
        //        filter.ToDate = DateTime.Now;
        //    else
        //        return FilterByFromDate(queryParameters, filter.FromDate);
        //}
        //public List<Operation> GetFilteredInfoAsync(QueryParameters queryParameters, Filter filter)
        //{
        //    IQueryable<Operation> historyPage;
        //    if (filter != null)
        //        historyPage = Filter(queryParameters, filter);
        //    else
        //        return GetAll(queryParameters);
        //    List<Operation> historyPage1 = historyPage
        //        .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
        //        .Take(queryParameters.PageCount).ToList();
        //    List<HistoryModel> history = _mapper.Map<List<HistoryModel>>(historyPage1);
        //    return history;
        //}
    }
}
