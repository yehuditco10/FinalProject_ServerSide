using Account.Services.Interfaces;
using AutoMapper;
using System.Linq;
using System.Linq.Dynamic.Core;
using Account.Services.Models.Pagination;
using System.Collections.Generic;
using System;

namespace Account.Data
{
    public class OperationHistoryRepository: IOperationHistoryRepository
    {
        private readonly AccountContext _accountContext;
        private readonly IMapper _mapper;

        public OperationHistoryRepository(AccountContext accountContext,IMapper mapper)
        {
            _accountContext = accountContext;
            _mapper = mapper;
        }
        public int GetCountPerAccount(Guid accountId)
        {
            return _accountContext.Operations.Where(a=>a.AccountId==accountId).Count();
        }
      
        public List<Services.Models.Operation> GetOperationsOrdered(QueryParameters queryParameters)
        {
            var totalCount = GetCountPerAccount(queryParameters.AccountId);
            IQueryable<Entities.Operation> _allItems;
            if (totalCount==0)
            {
                //to do
                throw new Exception("no content");
            }
            
            if (queryParameters.HasQuery())
            {
                var descending = queryParameters.IsDescending() == true ? "Desc" : "";
                _allItems = _accountContext.Operations.Where
                    (x => x.Id.ToString().Contains(queryParameters.Query.ToLowerInvariant())// וגם צריך להוסיף כזה משפט לכל פרמטר שרוצים לסדר לפיו.לא עושה הפוך
                && x.AccountId == queryParameters.AccountId)
                    .OrderBy(key => queryParameters.OrderBy);                
            }
            else
            {
                _allItems = _accountContext.Operations.Where(x=>x.AccountId == queryParameters.AccountId).OrderBy(key => queryParameters.OrderBy);
            }
            //var links = CreateLinksFsorCollection(queryParameters, totalCount);
            return _mapper.Map<List<Services.Models.Operation>>( _allItems);
        }

        //private List<Services.Models.Operation> ConvertFromQueryToList(QueryParameters queryParameters, IQueryable<Entities.Operation> _allItems)
        //{
        //    var query = _allItems
        //                    .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
        //                    .Take(queryParameters.PageCount);
        //    return _mapper.Map<List<Services.Models.Operation>>(query.ToList());
        //}

        private int List<T>(List<Entities.Operation> lists)
        {
            throw new NotImplementedException();
        }

        //private List<Link> CreateLinksForCollection(Services.Models.Pagination.QueryParameters queryParameters, int totalCount)
        //{
        //    var links = new List<Link>();

        //    links.Add(
        //     new Link(_urlHelper.Link(nameof(Add), null), "create", "POST"));

        //    // self 
        //    links.Add(
        //     new Link(_urlHelper.Link(nameof(GetAll), new
        //     {
        //         pagecount = queryParameters.PageCount,
        //         page = queryParameters.Page,
        //         orderby = queryParameters.OrderBy
        //     }), "self", "GET"));

        //    links.Add(new Link(_urlHelper.Link(nameof(GetAll), new
        //    {
        //        pagecount = queryParameters.PageCount,
        //        page = 1,
        //        orderby = queryParameters.OrderBy
        //    }), "first", "GET"));

        //    links.Add(new Link(_urlHelper.Link(nameof(GetAll), new
        //    {
        //        pagecount = queryParameters.PageCount,
        //        page = queryParameters.GetTotalPages(totalCount),
        //        orderby = queryParameters.OrderBy
        //    }), "last", "GET"));

        //    if (queryParameters.HasNext(totalCount))
        //    {
        //        links.Add(new Link(_urlHelper.Link(nameof(GetAll), new
        //        {
        //            pagecount = queryParameters.PageCount,
        //            page = queryParameters.Page + 1,
        //            orderby = queryParameters.OrderBy
        //        }), "next", "GET"));
        //    }

        //    if (queryParameters.HasPrevious())
        //    {
        //        links.Add(new Link(_urlHelper.Link(nameof(GetAll), new
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
    }
}
