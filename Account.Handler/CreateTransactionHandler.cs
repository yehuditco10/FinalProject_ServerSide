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
            TransactionCreated transactionCreated = new TransactionCreated()
            {
                TransactionId = message.TransactionId
            };
            //	Check correctness of accounts ids
            bool idsCorrectness = await _accountService.idsCorrectness(message.FromAccountId, message.ToAccountId);
            if (idsCorrectness == false)
            {
                transactionCreated.IsSucceeded = false;
                transactionCreated.FailureReason = "The accounts are invalid";
            }
            else
            {
                //	Check sender balance
                bool HasBalance = await _accountService.HasBalance(message.FromAccountId, message.Amount);
                if (HasBalance == false)
                {
                    transactionCreated.IsSucceeded = false;
                    transactionCreated.FailureReason = "There is not enough balance";
                }
                else
                {
                    //	Update receiver/sender balances (run in DB transaction)
                    bool isSucceeded = await _accountService.DoTransaction(
                        message.FromAccountId,
                        message.ToAccountId, message.Amount);
                    if (isSucceeded == false)
                    {
                        transactionCreated.IsSucceeded = false;
                        transactionCreated.FailureReason = "The transaction failed, even though it was valid ";
                    }
                    else
                    {
                        transactionCreated.IsSucceeded = true;
                    }
                }
            }
            //	Send result NSB event
            TransactionCreated transactionCreated = new TransactionCreated()
            {
                TransactionId = message.TransactionId,
                //todo
                IsSucceeded = true,
                FailureReason = "..."
            };
            await context.Publish(transactionCreated);
        }
    }
}
