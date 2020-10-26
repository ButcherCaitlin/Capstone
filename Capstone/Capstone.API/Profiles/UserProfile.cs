using AutoMapper;
using Capstone.API.Models;
using MongoDB.Driver.Core.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Models.CreateUserDto, Entities.User>();
            CreateMap<Entities.User, Models.OutboundUserDto>();
        }
    }
}
