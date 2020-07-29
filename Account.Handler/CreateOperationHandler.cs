using Account.Services.Interfaces;
using Account.Services.Models;
using AutoMapper;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Account.Handler
{
    public class CreateOperationHandler : IHandleMessages<TransactionSucceeded>
    {
        static ILog _log = LogManager.GetLogger<CreateOperationHandler>();
        private readonly IOperationHistoryService _operationHistoryService;
        private readonly IMapper _mapper;

        public CreateOperationHandler(IOperationHistoryService operationHistoryService,
            IMapper mapper)
        {
            _operationHistoryService = operationHistoryService;
            _mapper = mapper;
        }
        public async Task Handle(TransactionSucceeded message, IMessageHandlerContext context)
        {
            _log.Info("in createOperation");
            await _operationHistoryService.CreateOrerations(_mapper.Map<Operation>(message));
        }
    }
}
