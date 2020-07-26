using Account.Services;
using Account.Services.Models;
using Messages.Commands;
using Messages.Events;
using NServiceBus;
using System.Threading.Tasks;

namespace Account.Handler
{
    class CreateTransactionHandler : IHandleMessages<CreateTransaction>
    {
        private readonly IAccountService _accountService;

        public CreateTransactionHandler(IAccountService accountService)
        {
            _accountService = accountService;
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
            await _accountService.CreateTransaction(newTransaction);
        }
    }
}
