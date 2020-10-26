using AutoMapper;

namespace Capstone.API.Profiles
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            CreateMap<Models.CreatePropertyDto, Entities.Property>();
            CreateMap<Models.UpdatePropertyDto, Entities.Property>();
            CreateMap<Entities.Property, Models.OutboundPropertyDto>();
        }
    }
}
