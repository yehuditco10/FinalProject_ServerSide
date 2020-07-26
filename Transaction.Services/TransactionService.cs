﻿using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Transaction.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMessageSession _messageSession;

        public TransactionService(ITransactionRepository transactionRepository,
            IMessageSession messageSession)
        {
            _messageSession = messageSession;
            _transactionRepository = transactionRepository;
        }
        public async Task<bool> DoTransactionAsync(Models.Transaction transaction)
        {
            transaction.Id = new Guid();
            await AddTransactionToDB(transaction);
            SendDoTransactionMessage(transaction);
            return true;
        }
        private async Task<bool> AddTransactionToDB(Models.Transaction transaction)
        {
            transaction.Status = Models.eStatus.processing;
            return await _transactionRepository.AddTransactionToDB(transaction);
        }
        private void SendDoTransactionMessage(Models.Transaction transaction)
        {
            Messages.Commands.DoTransaction doTransaction = new Messages.Commands.DoTransaction()
            {
                FromAccountId = transaction.FromAccountId,
                ToAccountId = transaction.ToAccountId,
                Amount = transaction.Amount,
                Id = transaction.Id
            };
             _messageSession.Send(doTransaction).ConfigureAwait(false);
        }
    }
}
