using Account.Services.Interfaces;
using Account.Services.Models;
using AutoMapper;
using Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;
using System.Threading.Tasks;

namespace Account.Handler
{
    public class CreateOperationHandler : IHandleMessages<TransactionSucceeded>
    {
        static readonly ILog _log = LogManager.GetLogger<CreateOperationHandler>();
        private readonly IOperationService _operationHistoryService;
        private readonly IMapper _mapper;

        public CreateOperationHandler(IOperationService operationHistoryService,
            IMapper mapper)
        {
            _operationHistoryService = operationHistoryService;
            _mapper = mapper;
        }
        public async Task Handle(TransactionSucceeded message, IMessageHandlerContext context)
        {
            _log.Info("in createOperation");
            await _operationHistoryService.CreateOrerations(_mapper.Map<Transaction>(message));
        }
    }
}
