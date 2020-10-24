using System;
using System.Collections.Generic;
using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.API.Controllers
{
    [Route("api/properties")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly PropertyService _propertyService;
        private readonly IMapper _mapper;

        public PropertiesController(PropertyService propertyService,
            IMapper mapper)
        {
            _propertyService = propertyService ??
                throw new ArgumentNullException(nameof(propertyService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        //this will get all properties from the database
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<PropertyDto>> GetProperties()
        {
            var propertiesFromRepo = _propertyService.GetAll();
            if (propertiesFromRepo == null) return NotFound();
            return Ok(_mapper.Map<IEnumerable<PropertyDto>>(propertiesFromRepo));
        }
    }
}
