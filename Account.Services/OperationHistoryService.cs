using Account.Services.Interfaces;
using Account.Services.Models;
using Account.Services.Models.Pagination;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Services
{
    public class OperationHistoryService : IOperationHistoryService
    {
        private readonly IOperationHistoryRepository _operationHistoryRepository;
        public OperationHistoryService(IOperationHistoryRepository iOperationHistoryRepository)
        {
            _operationHistoryRepository = iOperationHistoryRepository;
        }
        public List<Operation> GetByParameters(QueryParameters queryParameters)
        {
            return _operationHistoryRepository.GetOperationsOrdered(queryParameters);
        }
        public PaginationMetadata PaginationMetadata(QueryParameters queryParameters)
        {
            PaginationMetadata paginationMetadata = new PaginationMetadata()
            {
                TotalCount = _operationHistoryRepository.GetCountPerAccount(queryParameters.AccountId),
                PageSize = queryParameters.PageCount,
                CurrentPage = queryParameters.Page,
            };
            paginationMetadata.TotalPages = queryParameters.GetTotalPages(paginationMetadata.TotalCount);
            return paginationMetadata;
        }
        public async Task CreateOrerations(Operation operations)
        {
            await _operationHistoryRepository.CreateOperation(operations.ToAccountId, operations.Amount, "credit", operations.TransactionId);
            await _operationHistoryRepository.CreateOperation(operations.FromAccountId, operations.Amount, "debit", operations.TransactionId);
        }
    }
}
