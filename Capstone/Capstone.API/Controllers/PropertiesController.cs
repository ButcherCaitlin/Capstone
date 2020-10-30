using System;
using System.Collections.Generic;
using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.ResourceParameters;
using Capstone.API.Services;
using Microsoft.AspNetCore.JsonPatch;
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
        [HttpGet("{propertyId:length(24)}", Name = "GetPropertyById")]
        [HttpHead("{propertyId:length(24)}")]
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
            if (userId == null) return BadRequest(new { message = "A UserID is required to create property records." });

            var propertyToAdd = _mapper.Map<Property>(property);
            propertyToAdd.OwnerID = userId;

            var createdProperty = _propertyService.Create(propertyToAdd);
            return CreatedAtRoute("GetPropertyById",
                new { propertyId = createdProperty.Id.ToString() },
                _mapper.Map<OutboundPropertyDto>(createdProperty));
        }

        [HttpPut("{propertyId:length(24)}")]
        public IActionResult UpdateProperty(string propertyId, UpdatePropertyDto property,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to Modify property records." });

            var propertyFromRepo = _propertyService.Get(propertyId);
            if (propertyFromRepo == null)
            {
                var propertyToAdd = _mapper.Map<Property>(property);
                propertyToAdd.Id = propertyId;
                propertyToAdd.OwnerID = userId;
                _propertyService.Create(propertyToAdd);

                return CreatedAtRoute("GetPropertyById",
                    new { propertyId },
                    _mapper.Map<OutboundPropertyDto>(propertyToAdd));
            }

            if (propertyFromRepo.OwnerID != userId) return BadRequest(new { message = "Only the owner of a property is allowed to modify its fields. " });

            _mapper.Map(property, propertyFromRepo);
            propertyFromRepo.OwnerID = userId;

            _propertyService.Update(propertyFromRepo.Id, propertyFromRepo);

            return NoContent();
        }


        [HttpPatch("{propertyId:length(24)}")]
        public IActionResult PartiallyUpdateProperty(string propertyId,
            JsonPatchDocument<UpdatePropertyDto> patchDocument,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to Modify property records." });

            var propertyToPatch = _propertyService.Get(propertyId);
            if (propertyToPatch == null) return NotFound(new { message = "If you are trying to upsert a resource use PUT" });


            if (propertyToPatch.OwnerID != userId)
            {
                return BadRequest(new { message = "Only the owner of a property is allowed to modify its fields. " });
            }

            var propertyDtoToPatch = _mapper.Map<UpdatePropertyDto>(propertyToPatch);
            patchDocument.ApplyTo(propertyDtoToPatch, ModelState);

            if (!TryValidateModel(propertyDtoToPatch)) return ValidationProblem(ModelState);

            _mapper.Map(propertyDtoToPatch, propertyToPatch);
            _propertyService.Update(propertyToPatch.Id, propertyToPatch);

            return NoContent();
        }

        [HttpDelete("{propertyId}")]
        public IActionResult DeleteProperty(string propertyId,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to delete property records." });

            var propertyToDelete = _propertyService.Get(propertyId);
            if (propertyToDelete == null) return NotFound();

            if (propertyToDelete.OwnerID != userId)
            {
                return BadRequest(new { message = "Only the owner of a property is allowed to delete its fields. " });
            }

            _propertyService.Remove(propertyToDelete.Id);
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetPropertiesOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS,HEAD");
            return Ok();
        }
    }
}
