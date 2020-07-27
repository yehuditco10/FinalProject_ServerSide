using Account.Api.DTO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Account.Handler
{
   public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, Services.Models.Customer>();
            CreateMap<Services.Models.Customer, Customer>();
            CreateMap<Data.Entities.Customer, Services.Models.Customer>();
            CreateMap<Services.Models.Customer, Data.Entities.Customer>();
            CreateMap<Data.Entities.Account, Services.Models.Account>();
            CreateMap<Services.Models.Account, Data.Entities.Account>();

        }
    }
}
