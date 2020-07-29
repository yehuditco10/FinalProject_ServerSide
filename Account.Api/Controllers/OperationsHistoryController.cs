using System;
using System.Collections.Generic;
using System.Linq;
using Account.Api.DTO;
using Account.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Account.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsHistoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOperationService _operationHistoryService;

        public OperationsHistoryController(IOperationService historyService,
            IMapper imapper)
        {
            _mapper = imapper;
            _operationHistoryService = historyService;
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] QueryParameters queryParameters)
        {
            queryParameters.Query += "OperationDate desc";
            var queryParametersModel = _mapper.Map<Services.Models.Pagination.QueryParameters>(queryParameters);

            var allOperation = _operationHistoryService.GetByParameters(queryParametersModel);
            var paginationMetadata = _operationHistoryService.PaginationMetadata(queryParametersModel);
            Response.Headers
                .Add("X-Pagination",
                    JsonConvert.SerializeObject(paginationMetadata));
           
            //var toReturn = allCustomers.Select(x => ExpandSingleItem(x));
            return Ok(new
            {
                value = allOperation,
             //   links = links
            });
        }
    }
} 

