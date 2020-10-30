using AutoMapper;
using System.Reflection.Metadata;

namespace Capstone.API.Profiles
{
    public class ShowingProfile : Profile 
    {
        public ShowingProfile()
        {
            CreateMap<Models.CreateShowingDto, Entities.Showing>();
            CreateMap<Entities.Showing, Models.UnformattedShowingDto>();
            CreateMap<Entities.Showing, Models.RealtorShowingDto>()
                .ForMember(
                    dest => dest.CounterpartID,
                    opt => opt.MapFrom(src => src.ProspectID));
            CreateMap<Entities.Showing, Models.ProspectShowingDto>()
                .ForMember(
                    dest => dest.CounterpartID,
                    opt => opt.MapFrom(src => src.RealtorID));
            CreateMap<Models.UpdateShowingDto, Entities.Showing>();
            CreateMap<Entities.Showing, Models.UpdateShowingDto>();
        }
    }
}
