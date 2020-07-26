using AutoMapper;

namespace Transaction.Api
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<DTO.Transaction, Services.Models.Transaction>();
            CreateMap<Services.Models.Transaction, Data.Entities.Transaction>();
        }
    }
}
