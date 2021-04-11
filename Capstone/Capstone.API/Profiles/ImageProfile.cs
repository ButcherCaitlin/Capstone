using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;


namespace Capstone.API.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Models.Image.CreateImageDto, Entities.Image>();
            CreateMap<Models.Image.UpdateImageDto, Entities.Image>();
            CreateMap<Entities.Image, Models.Image.OutboundImageDto>();
            CreateMap<Entities.Image, Models.Image.UpdateImageDto>();
        }
    }
}
