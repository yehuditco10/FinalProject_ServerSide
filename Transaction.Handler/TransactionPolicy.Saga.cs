using Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transaction.Handler
{
    public class TransactionPolicy : Saga<TransactionPolicyData>,
        IAmStartedByMessages<Messages.Commands.DoTransaction>
    {
        static ILog _log = LogManager.GetLogger<TransactionPolicy>();
        public async Task Handle(DoTransaction message, IMessageHandlerContext context)
        {
            _log.Error($"Received DoTransaction in Transaction saga");
            await context.Send(message).ConfigureAwait(false);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<TransactionPolicyData> mapper)
        {
            mapper.ConfigureMapping<DoTransaction>(message => message.Id)
                                   .ToSaga(sagaData => sagaData.Id);
        }
    }
}
