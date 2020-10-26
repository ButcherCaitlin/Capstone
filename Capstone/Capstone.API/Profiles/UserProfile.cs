using AutoMapper;

namespace Capstone.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Models.CreateUserDto, Entities.User>();
            CreateMap<Models.UpdateUserDto, Entities.User>();
            CreateMap<Entities.User, Models.OutboundUserDto>();
        }
    }
}
