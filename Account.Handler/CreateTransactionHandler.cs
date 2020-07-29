using Account.Services.Interfaces;
using Account.Services.Models;
using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Account.Handler
{
    class CreateTransactionHandler : IHandleMessages<CreateTransaction>

    {
        private readonly ITransferenceService _transferenceService;
        static readonly ILog _log = LogManager.GetLogger<CreateTransactionHandler>();

        public CreateTransactionHandler(ITransferenceService transferenceService)
        {
            _transferenceService = transferenceService;
        }
        public async Task Handle(CreateTransaction message, IMessageHandlerContext context)
        {
            _log.Info($"The transaction was recorded, starting the transfer process between {message.FromAccountId} and {message.ToAccountId} ");
            Transaction newTransaction = new Transaction()
            {
                TransactionId=message.TransactionId,
                ToAccountId=message.ToAccountId,
                FromAccountId=message.FromAccountId,
                Amount=message.Amount
            };
           TransactionCreated transactionCreated = await _transferenceService.CreateTransaction(newTransaction);
            
            if(transactionCreated.IsSucceeded==true)
            {
                TransactionSucceeded transactionSucceeded = new TransactionSucceeded()
                {
                    TransactionId = transactionCreated.TransactionId,
                    FromAccountId = message.FromAccountId,
                    ToAccountId = message.ToAccountId,
                    Amount = message.Amount
                };
                await context.Publish(transactionSucceeded).ConfigureAwait(false);
            }
            _log.Info("publish transactionCreated");
           await context.Publish(transactionCreated).ConfigureAwait(false);
        }
    }
}
