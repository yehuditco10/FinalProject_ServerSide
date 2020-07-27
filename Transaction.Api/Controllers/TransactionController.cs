using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Transaction.Services;

namespace Transaction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public TransactionController(IMapper mapper,
            ITransactionService transactionService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }

        [HttpPost]
        public ActionResult DoTransaction(DTO.Transaction transaction)
        {
            var res = _transactionService.DoTransactionAsync(_mapper.Map<Services.Models.Transaction>(transaction));
            return Ok(res.Result);
        }
    }
}