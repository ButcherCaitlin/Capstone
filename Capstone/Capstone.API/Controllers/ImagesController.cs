using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using Capstone.API.Services;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Capstone.API.Models.Image;

namespace Capstone.API.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly DataService _dataService;
        private readonly IMapper _mapper;
        public ImagesController(DataService dataService,
            IMapper mapper)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{imageId}", Name = "GetImageById")]
        [HttpHead("{imageId}")]
        public ActionResult<OutboundPropertyDto> GetImageById(string imageId)
        {
            var imageFromRepo = _dataService.GetImage(imageId);
            if (imageFromRepo == null) return NotFound();

            return Ok(_mapper.Map<OutboundImageDto>(imageFromRepo));
        }

        [HttpPost]
        public ActionResult<OutboundImageDto> CreateImage(CreateImageDto image)
        {
          

            var imageToAdd = _mapper.Map<Image>(image);

            var createdImage= _dataService.Create(imageToAdd);

            return CreatedAtRoute("GetImageById",
                new { propertyId = createdImage.Id.ToString() },
                _mapper.Map<OutboundPropertyDto>(createdImage));
        }
    }
}
