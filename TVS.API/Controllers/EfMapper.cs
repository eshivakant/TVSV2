using AutoMapper;
using Microsoft.Owin.BuilderProperties;
using TVS.API.Entities;
using Address = TVS.API.Entities.Address;

namespace TVS.API.Controllers
{
    public static class EfMapper
    {
        private static IMapper _mapper;
        static EfMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Address, Address>().ForMember(a=>a.AddressOccupations, b=>b.Ignore()).ForMember(a=>a.AddressOwnerships, b=>b.Ignore());
                cfg.CreateMap<Person, Person>().ForMember(a => a.AddressOccupations, b => b.Ignore()).ForMember(a => a.AddressOwnerships, b => b.Ignore()).ForMember(a => a.DomainAspnetPersonMaps, b => b.Ignore());
                cfg.CreateMap<AddressOwnership, AddressOwnership>().ForMember(a => a.Address, b => b.Ignore()).ForMember(a => a.Person, b => b.Ignore());
                cfg.CreateMap<AddressOccupation, AddressOccupation>().ForMember(a => a.Address, b => b.Ignore()).ForMember(a => a.Person, b => b.Ignore());
                cfg.CreateMap<PersonAttribute, PersonAttribute>().ForMember(a => a.Person, b => b.Ignore()).ForMember(a => a.RoleAttribute, b => b.Ignore());
                cfg.CreateMap<RoleAttribute, RoleAttribute>().ForMember(a => a.PersonAttributes, b => b.Ignore());
                cfg.CreateMap<RoleParameter, RoleParameter>().ForMember(a => a.RatingBreakdowns, b => b.Ignore());
            });

             _mapper = config.CreateMapper();
        }

        public static void Map<T1,T2>(T1 source, T2 dest)
        {
            _mapper.Map<T1, T2>(source, dest);
        }
    }
}
