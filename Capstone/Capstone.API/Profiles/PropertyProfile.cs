using AutoMapper;

namespace Capstone.API.Profiles
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Entities.Property, Models.PropertyDto>();
            CreateMap<Models.PropertyDto, Entities.Property>();
        }
    }
}
