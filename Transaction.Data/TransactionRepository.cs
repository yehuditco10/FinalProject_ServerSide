using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
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
            try
            {
                Entities.Transaction transaction = await _transactionContext.Transactions
                                .FirstOrDefaultAsync(m => m.Id == transactionId);
                if (transaction != null)
                {
                    transaction.Status = status;
                    return await _transactionContext.SaveChangesAsync();
                }
                throw new Exceptions.TransactionNotFoundExeption("This transaction id not found");
            }
            catch (Exception e)
            {
                throw new Exceptions.AddToDBFailedExeption(e.Message);
            }

        }
        public async Task<bool> AddTransactionToDB(Services.Models.Transaction transaction)
        {
            try
            {
                var e = _transactionContext.Transactions.Add(_mapper.Map<Entities.Transaction>(transaction));
                return await _transactionContext.SaveChangesAsync() > 0;
            }
            catch (Exception e)
            {
                throw new Exceptions.AddToDBFailedExeption(e.Message, e.InnerException);
            }
        }
    }
}
