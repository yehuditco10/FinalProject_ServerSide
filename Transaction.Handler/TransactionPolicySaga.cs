using AutoMapper;
using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;
using Transaction.Services;
using Transaction.Services.Models;

namespace Transaction.Handler
{
    public class TransactionPolicy : Saga<TransactionPolicyData>,
        IAmStartedByMessages<CreateTransaction>,
        IHandleMessages<TransactionCreated>
    {
        static readonly ILog _log = LogManager.GetLogger<TransactionPolicy>();
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionPolicy(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }
        public async Task Handle(CreateTransaction message, IMessageHandlerContext context)
        {
            _log.Info("Received DoTransaction in Transaction saga");
            await context.Send(message).ConfigureAwait(false);
        }

        public async Task Handle(TransactionCreated message, IMessageHandlerContext context)
        {
            _log.Info($"recived transaction created back to saga, succeeded? - {message.IsSucceeded}, reason: {message.FailureReason}");
            await _transactionService.UpdateStatus(_mapper.Map<TransactionStatus>(message));
            //return  Task.CompletedTask;
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TransactionPolicyData> mapper)
        {
            mapper.ConfigureMapping<CreateTransaction>(message => message.TransactionId)
                                   .ToSaga(sagaData => sagaData.TransactionId);
            mapper.ConfigureMapping<TransactionCreated>(message => message.TransactionId)
                                   .ToSaga(sagaData => sagaData.TransactionId);
        }
    }
}
