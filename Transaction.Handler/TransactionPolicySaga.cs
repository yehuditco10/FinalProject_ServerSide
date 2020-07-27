﻿using Messages.Commands;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Transaction.Handler
{
    public class TransactionPolicy : Saga<TransactionPolicyData>,
        IAmStartedByMessages<CreateTransaction>,
        IHandleMessages<TransactionCreated>
    {
        static ILog _log = LogManager.GetLogger<TransactionPolicy>();
        public async Task Handle(CreateTransaction message, IMessageHandlerContext context)
        {
            _log.Error("Received DoTransaction in Transaction saga");
            await context.Send(message).ConfigureAwait(false);
        }

        public Task Handle(TransactionCreated message, IMessageHandlerContext context)
        {
            _log.Error($"recived transaction created back to saga, succeeded? - {message.IsSucceeded}, reason: {message.FailureReason}");
           return Task.CompletedTask;
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
