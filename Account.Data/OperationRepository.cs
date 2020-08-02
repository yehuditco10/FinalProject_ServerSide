using Account.Services.Interfaces;
using Account.Services.Models.Pagination;
using AutoMapper;
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

        public OperationRepository(AccountContext accountContext, IMapper mapper)
        //,IUrlHelper iURLHelper   
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
          
            _allItems = FilterResult(queryParameters);
            if (_allItems.Count() == 0)
            {
                return _mapper.Map < List < Services.Models.Operation >> (_allItems);
            }
            //  var links = CreateLinksForCollection(queryParameters, totalCount);
            var query = _allItems
                            .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                            .Take(queryParameters.PageCount);
            return _mapper.Map<List<Services.Models.Operation>>(query);
        }
        private IQueryable<Entities.Operation> FilterResult(QueryParameters queryParameters)
        {
            DateTime emptyDate = DateTime.MinValue;
            IQueryable<Entities.Operation> res = _accountContext.Operations.
                Where(id => id.AccountId == queryParameters.AccountId)
                .OrderBy(queryParameters.OrderBy, queryParameters.IsDescending());

            if (queryParameters.FromDate != emptyDate && queryParameters.ToDate == emptyDate)
            {
                queryParameters.ToDate = DateTime.Now;

                if (!String.IsNullOrEmpty(queryParameters.Type))
                {
                    res = FilterByDatesAndType(res, queryParameters);
                }
                else
                {
                    res = ByTime(res, queryParameters);
                }
            }
            if (!String.IsNullOrEmpty(queryParameters.Type))
            {
                res = IsCredit(res, queryParameters);
            }
            return res;
        }
        private IQueryable<Entities.Operation> IsCredit(IQueryable<Entities.Operation> list, QueryParameters queryParameters)
        {
            return list.Where(t => t.IsCredit == (queryParameters.Type == "credit"));
        }
        private IQueryable<Entities.Operation> ByTime(IQueryable<Entities.Operation> list, QueryParameters queryParameters)
        {
            return list.Where(t => t.OperationTime >= queryParameters.FromDate &&
            t.OperationTime <= queryParameters.ToDate);
        }
        private IQueryable<Entities.Operation> FilterByDatesAndType(IQueryable<Entities.Operation> list, QueryParameters queryParameters)
        {
            return list.Where(t => t.IsCredit == (queryParameters.Type == "credit") &&
            t.OperationTime >= queryParameters.FromDate &&
            t.OperationTime <= queryParameters.ToDate);
        }
        public async Task CreateOperation(Services.Models.Operation operation)
        {
            Entities.Operation newOperation = _mapper.Map<Entities.Operation>(operation);
            Entities.Account account = await _accountContext.Accounts.FirstOrDefaultAsync(a => a.Id == operation.AccountId);
            newOperation.Id = Guid.NewGuid();
            newOperation.Balance = account.Balance;
            newOperation.OperationTime = DateTime.Now;
            _accountContext.Operations.Add(newOperation);
            await _accountContext.SaveChangesAsync();
        }

        //public void Add(Entities.Operation item)
        //{
        //    _accountContext.Operations.Add(item);
        //}
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
    }
}