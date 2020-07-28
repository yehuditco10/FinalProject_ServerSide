using NServiceBus;
using System;
using System.Threading.Tasks;
using Transaction.Services.Models;

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
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<bool> DoTransactionAsync(Models.Transaction transaction)
        {

            transaction.Id = Guid.NewGuid();
            transaction.Status = Models.eStatus.processing;
            transaction.Date = DateTime.Now;
            await AddTransactionToDB(transaction);
            SendDoTransactionMessage(transaction);
            return true;
        }

        public async Task UpdateStatus(TransactionStatus transactionStatus)
        {
           await _transactionRepository.UpdateStatus(transactionStatus);
        }

        private async Task<bool> AddTransactionToDB(Models.Transaction transaction)
        {
            
            return await _transactionRepository.AddTransactionToDB(transaction);
        }
        private void SendDoTransactionMessage(Models.Transaction transaction)
        {
            Messages.Commands.CreateTransaction doTransaction = new Messages.Commands.CreateTransaction()
            {
                FromAccountId = transaction.FromAccountId,
                ToAccountId = transaction.ToAccountId,
                Amount = transaction.Amount,
                TransactionId = transaction.Id
            };
             _messageSession.Send(doTransaction).ConfigureAwait(false);
        }
    }
}
