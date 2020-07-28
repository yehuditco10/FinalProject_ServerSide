using Account.Services.Interfaces;
using Account.Services.Models;
using AutoMapper;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Account.Handler
{
    public class CreateOperationHandler : IHandleMessages<TransactionSucceeded>
    {
        static ILog _log = LogManager.GetLogger<CreateOperationHandler>();
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public CreateOperationHandler(IAccountService accountService,
            IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }
        public async Task Handle(TransactionSucceeded message, IMessageHandlerContext context)
        {
            _log.Info("in createOperation");
            await _accountService.CreateOrerations(_mapper.Map<Operations>(message));
        }
    }
}
