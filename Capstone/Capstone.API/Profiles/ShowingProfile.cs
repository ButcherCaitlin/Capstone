using AutoMapper;

namespace Capstone.API.Profiles
{
    public class ShowingProfile : Profile 
    {
        public ShowingProfile()
        {
            CreateMap<Entities.Showing, Models.ShowingForRealtorDto>()
                .ForMember(
                    dest => dest.CounterpartID,
                    opt => opt.MapFrom(src => src.ProspectID));
            CreateMap<Entities.Showing, Models.ShowingForProspectDto>()
                .ForMember(
                    dest => dest.CounterpartID,
                    opt => opt.MapFrom(src => src.RealtorID));
        }
    }
}
