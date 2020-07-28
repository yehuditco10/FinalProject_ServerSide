﻿using Account.Services.Models;
using Account.Services.Models.Pagination;
using System.Collections.Generic;

namespace Account.Services.Interfaces
{
    public interface IOperationHistoryService
    {
        List<Operation> GetByParameters(QueryParameters queryParameters);
        PaginationMetadata PaginationMetadata(QueryParameters queryParameters);
    }
}