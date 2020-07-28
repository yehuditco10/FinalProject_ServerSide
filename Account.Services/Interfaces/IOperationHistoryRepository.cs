using Account.Services.Models;
using Account.Services.Models.Pagination;
using System;
using System.Collections.Generic;

namespace Account.Services.Interfaces
{
    public interface IOperationHistoryRepository
    {
        int GetCountPerAccount(Guid accountId);
        List<Operation> GetOperationsOrdered(QueryParameters queryParameters);
    }
}
