using Account.Api.DTO;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace Account.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, Services.Models.Customer>();
            CreateMap<Services.Models.Customer, Customer>();
            CreateMap< Entities.Customer,Services.Models.Customer>();
            CreateMap<Services.Models.Customer,Entities.Customer>();
            CreateMap<Entities.Account, Services.Models.Account>();
            CreateMap<Services.Models.Account, Entities.Account>();
            CreateMap<QueryParameters, Services.Models.Pagination.QueryParameters>();
            CreateMap<Entities.Operation, Services.Models.Operation>();
            CreateMap<List<Entities.Operation>,List< Services.Models.Operation>>();
            CreateMap<IQueryable<Entities.Operation>,List< Services.Models.Operation>>();
        }
    }
}
