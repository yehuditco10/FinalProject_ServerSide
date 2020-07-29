using Account.Services.Interfaces;
using Account.Services.Models;
using Account.Services.Models.Pagination;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Services
{
    public class OperationService : IOperationService
    {
        private readonly IOperationRepository _operationHistoryRepository;
        public OperationService(IOperationRepository iOperationHistoryRepository)
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
        public async Task CreateOrerations(Transaction transaction)
        {
            Operation operatoinTo = new Operation()
            {
                AccountId = transaction.ToAccountId,
                Amount = transaction.Amount,
                IsCredit = true,
                TransactionId = transaction.TransactionId
            };
            await _operationHistoryRepository.CreateOperation(operatoinTo);
            Operation operatoinFrom = new Operation()
            {
                AccountId = transaction.FromAccountId,
                Amount = transaction.Amount,
                IsCredit = false,
                TransactionId = transaction.TransactionId
            };
            await _operationHistoryRepository.CreateOperation(operatoinFrom);
        }
    }
}
