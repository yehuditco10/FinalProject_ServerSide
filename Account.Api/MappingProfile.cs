using Account.Api.DTO;
using Account.Services.Models;
using AutoMapper;
using Messages.Events;
using System;
using System.Collections.Generic;
using System.Text;

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
            //?here
            CreateMap<TransactionSucceeded, Operations>();

        }
    }
}
