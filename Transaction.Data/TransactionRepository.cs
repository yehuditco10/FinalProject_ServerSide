using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Transaction.Data.Exceptions;
using Transaction.Services;
using Transaction.Services.Models;

namespace Transaction.Data
{
    public class TransactionRepository : ITransactionRepository
    {
        private TransactionContext _transactionContext { get; }
        private readonly IMapper _mapper;
        public TransactionRepository(TransactionContext transactionContext,
           IMapper imapper)
        {
            _transactionContext = transactionContext;
            _mapper = imapper;
        }
        public async Task<int> UpdateStatus(Guid transactionId, eStatus status)
        {
            Entities.Transaction transaction = await _transactionContext.Transactions
                            .FirstOrDefaultAsync(m => m.Id == transactionId);
            if (transaction != null)
            {
                transaction.Status = status;
                return await _transactionContext.SaveChangesAsync();
            }
            throw new TransactionNotFoundExeption("This transaction id not found");
        }
        public async Task<bool> AddTransactionToDB(Services.Models.Transaction transaction)
        {
            _transactionContext.Transactions.Add(_mapper.Map<Entities.Transaction>(transaction));
            return await _transactionContext.SaveChangesAsync() > 0;
        }

        public async Task UpdateStatus(TransactionStatus transactionStatus)
        {
            var transaction = await _transactionContext.Transactions.FirstOrDefaultAsync(
                t => t.Id == transactionStatus.TransactionId);
            if (transaction == null)
            {
                throw new TransactionNotFoundExeption("TransactionId is not valid");
            }
            if (transactionStatus.isSucceeded == false)
            {
                transaction.Status = eStatus.failed;
                transaction.FailureReason = transactionStatus.FailureReason;
            }
            else
                transaction.Status = eStatus.successed;
        }
    }
}
