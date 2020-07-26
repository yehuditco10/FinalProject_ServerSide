using Account.Services.Models;
using Account.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Messages.Events;
using NServiceBus;

namespace Account.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMessageSession _messageSession;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(IMessageSession messageSession,
            ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
            _messageSession = messageSession;
        }
        public async Task CreateTransaction(Transaction transaction)
        {
            TransactionCorrectness transactionCorrectness = await AccountIdsCorrectness(transaction.FromAccountId, transaction.ToAccountId);
            if (transactionCorrectness.IsValid == true)
            {
                transactionCorrectness = await HasBalance(transaction.FromAccountId, transaction.Amount);
                if (transactionCorrectness.IsValid == true)
                {
                    transactionCorrectness = await DoTransaction(transaction);
                }
            }
            TransactionCreated transactionCreated = new TransactionCreated()
            {
                TransactionId = transaction.TransactionId,
                IsSucceeded = transactionCorrectness.IsValid,
                FailureReason = transactionCorrectness.Reason
            };
            await _messageSession.Publish(transactionCreated);
        }

        private Task<TransactionCorrectness> AccountIdsCorrectness(Guid fromAccountId, Guid toAccountId)
        {
            _transactionRepository.IsAccountExistsAsync
        }
        private Task<TransactionCorrectness> HasBalance(Guid fromAccountId, int amount)
        {
            throw new NotImplementedException();
        }
        private Task<TransactionCorrectness> DoTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
