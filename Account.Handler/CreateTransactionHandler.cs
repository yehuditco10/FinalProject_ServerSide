using Account.Services.Interfaces;
using Account.Services.Models;
using Messages.Commands;
using NServiceBus;
using System.Threading.Tasks;

namespace Account.Handler
{
    class CreateTransactionHandler : IHandleMessages<CreateTransaction>
    {
        private readonly ITransactionService _transactionService;

        public CreateTransactionHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public async Task Handle(CreateTransaction message, IMessageHandlerContext context)
        {
            Transaction newTransaction = new Transaction()
            {
                TransactionId=message.TransactionId,
                ToAccountId=message.ToAccountId,
                FromAccountId=message.FromAccountId,
                Amount=message.Amount
            };
            await _transactionService.CreateTransaction(newTransaction);
        }
    }
}
