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
                    await DoTransaction(transaction);
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
        private async Task<TransactionCorrectness> AccountIdsCorrectness(Guid fromAccountId, Guid toAccountId)
        {
            TransactionCorrectness transactionCorrectness = new TransactionCorrectness();
            if (await _transactionRepository.IsAccountExistsAsync(fromAccountId) == false)
            {
                transactionCorrectness.Reason = "The fromAccountId doesn't exist";
            }
            else if (await _transactionRepository.IsAccountExistsAsync(toAccountId) == false)
            {
                transactionCorrectness.Reason = "The toAccountId doesn't exist";
            }
            return transactionCorrectness;
        }
        private async Task<TransactionCorrectness> HasBalance(Guid fromAccountId, int amount)
        {
            TransactionCorrectness transactionCorrectness = new TransactionCorrectness();
            int balance = await _transactionRepository.GetBalance(fromAccountId);
            if (balance < amount)
            {
                transactionCorrectness.IsValid = false;
                transactionCorrectness.Reason = "There is not enough money in the account";
            }
            return transactionCorrectness;
        }
        private Task DoTransaction(Transaction transaction)
        {
            _transactionRepository.DoTransaction(transaction);
            return Task.CompletedTask;
        }
    }
}
