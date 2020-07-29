using Account.Services.Models;
using Account.Services.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Account.Services.Interfaces
{
    public interface IOperationRepository
    {
        int GetCountPerAccount(Guid accountId);
        List<Operation> GetOperationsOrdered(QueryParameters queryParameters);
        Task CreateOperation(Operation operation);
    }
}
