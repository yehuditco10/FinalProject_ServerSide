using Account.Api.DTO;
using Account.Services.Models;
using AutoMapper;
using Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Account.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Api.DTO.Customer, Services.Models.Customer>();
            CreateMap<Services.Models.Customer, Api.DTO.Customer>();
            CreateMap< Entities.Customer,Services.Models.Customer>();
            CreateMap<Services.Models.Customer,Entities.Customer>();
            CreateMap<Entities.Account, Services.Models.Account>();
            CreateMap<Services.Models.Account, Entities.Account>();
            CreateMap<QueryParameters, Services.Models.Pagination.QueryParameters>();
            CreateMap<Services.Models.Operation,Entities.Operation >();
            CreateMap<Services.Models.Operation, Api.DTO.Operation>()
               .ForMember(dto => dto.Date, model => model.MapFrom(time => time.OperationTime));
            CreateMap<Entities.Operation,Services.Models.Operation>();
            //?here
            CreateMap<TransactionSucceeded, Services.Models.Transaction>();

        }
    }
}
