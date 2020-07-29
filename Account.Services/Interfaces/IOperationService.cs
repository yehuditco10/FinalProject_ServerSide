using Account.Services.Models;
using Account.Services.Models.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Account.Services.Interfaces
{
    public interface IOperationService
    {
        List<Operation> GetByParameters(QueryParameters queryParameters);
        PaginationMetadata PaginationMetadata(QueryParameters queryParameters);
        Task CreateOrerations(Transaction operation);
    }
}
