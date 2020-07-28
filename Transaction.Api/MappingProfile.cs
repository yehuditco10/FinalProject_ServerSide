using AutoMapper;
using Messages.Events;
using Transaction.Services.Models;

namespace Transaction.Api
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<DTO.Transaction, Services.Models.Transaction>();
            CreateMap<Services.Models.Transaction, Data.Entities.Transaction>();
            CreateMap<TransactionCreated, TransactionStatus>();
        }
    }
}
