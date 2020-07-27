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
        private readonly ITransferenceService _transactionService;
        static ILog _log = LogManager.GetLogger<CreateTransactionHandler>();

        public CreateTransactionHandler(ITransferenceService transactionService)
        {
            _transactionService = transactionService;
        }
        public async Task Handle(CreateTransaction message, IMessageHandlerContext context)
        {
            _log.Error("got create transaction from transaction service!");
            Transaction newTransaction = new Transaction()
            {
                TransactionId=message.TransactionId,
                ToAccountId=message.ToAccountId,
                FromAccountId=message.FromAccountId,
                Amount=message.Amount
            };
           TransactionCreated transactionCreated = await _transactionService.CreateTransaction(newTransaction);
            _log.Error("publish transactionCreated");
            await context.Publish(transactionCreated);
        }
    }
}
