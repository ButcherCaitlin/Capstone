using AutoMapper;
using System.Reflection.Metadata;

namespace Capstone.API.Profiles
{
    public class ShowingProfile : Profile 
    {
        public ShowingProfile()
        {
            CreateMap<Models.CreateShowingDto, Entities.Showing>();
            CreateMap<Models.UpdateShowingDto, Entities.Showing>();
            CreateMap<Entities.Showing, Models.OutboundShowingDto>();
        }
    }
}
