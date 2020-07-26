using Messages.Commands;
using Messages.Events;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
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
            //	Check correctness of accounts ids
            // bool idsCorrectness= await 

            //	Check sender balance
            //bool idHasBalance=
            //	Update receiver/sender balances (run in DB transaction)

            //	Send result NSB event
            TransactionCreated transactionCreated = new TransactionCreated()
            {
                TransactionId = message.Id,
                //todo
                IsSucceeded = true,
                FailureReason = "..."
            };
            await context.Publish(transactionCreated);
        }
    }
}
