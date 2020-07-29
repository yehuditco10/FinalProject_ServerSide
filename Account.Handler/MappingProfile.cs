using Account.Api.DTO;
using Account.Services.Models;
using AutoMapper;
using Messages.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Handler
{
   public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Api.DTO.Customer, Services.Models.Customer>();
            CreateMap<Services.Models.Customer, Api.DTO.Customer>();
            CreateMap<Data.Entities.Customer, Services.Models.Customer>();
            CreateMap<Services.Models.Customer, Data.Entities.Customer>();
            CreateMap<Data.Entities.Account, Services.Models.Account>();
            CreateMap<Services.Models.Account, Data.Entities.Account>();
            CreateMap<Services.Models.Operation, Data.Entities.Operation>();
            CreateMap<Data.Entities.Operation, Services.Models.Operation>();
            CreateMap<TransactionSucceeded, Services.Models.Transaction>();
        }
    }
}
