using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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
        public async Task<ActionResult<bool>> DoTransaction(DTO.Transaction transaction)
        {
            if (transaction.FromAccountId == transaction.ToAccountId)
                return BadRequest("Not Makes sense to transfer to yourself");
            var res = await _transactionService.DoTransactionAsync(_mapper.Map<Services.Models.Transaction>(transaction));
            return res;
        }
    }
}