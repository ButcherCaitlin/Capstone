using System;
using System.Collections.Generic;
using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.ResourceParameters;
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

        /// <summary>
        /// Returns a set properties based on a set of filtering and search parameters sepecified in the query string.
        /// </summary>
        /// <param name="parameters">The the filtering and searching parameters.</param>
        /// <returns>The set of properties specified by the parameters.</returns>
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<OutboundPropertyDto>> GetProperties(
            [FromQuery] PropertiesResourceParameters parameters = null)
        {
            var propertiesFromRepo = _propertyService.Get(parameters);
            if (propertiesFromRepo == null) return NotFound();
            return Ok(_mapper.Map<IEnumerable<OutboundPropertyDto>>(propertiesFromRepo));
        }
        /// <summary>
        /// Returns a property with the ID provided in the route.
        /// </summary>
        /// <param name="propertyId">The ID of the property to be found. Provided in the route.</param>
        /// <returns>The property object.</returns>
        [HttpGet("{propertyId}", Name = "GetPropertyById")]
        [HttpHead("{propertyId}")]
        public ActionResult<OutboundPropertyDto> GetPropertyById(string propertyId)
        {
            var propertyFromRepo = _propertyService.Get(propertyId);
            if (propertyFromRepo == null) return NotFound();
            return Ok(_mapper.Map<OutboundPropertyDto>(propertyFromRepo));
        }
        /// <summary>
        /// Creates a single property, and returns the created property to the consumer. Also returns the route to the property.
        /// </summary>
        /// <param name="property">The property to create. Found in the body of the request</param>
        /// <returns>The newly created property from the databse</returns>
        [HttpPost]
        public ActionResult<OutboundPropertyDto> CreateProperty(CreatePropertyDto property,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest("A UserID is required to create property records.");

            var propertyToAdd = _mapper.Map<Property>(property);
            propertyToAdd.OwnerID = userId;

            var createdProperty = _propertyService.CreateOne(propertyToAdd);
            return CreatedAtRoute("GetPropertyById", 
                new { propertyId = createdProperty.Id.ToString() },
                _mapper.Map<OutboundPropertyDto>(createdProperty));
        }

        [HttpOptions]
        public IActionResult GetPropertiesOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS,HEAD");
            return Ok();
        }
    }
}
