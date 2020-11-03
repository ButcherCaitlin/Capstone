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
        /// Queries Property records from the database based off of a set of query parameters.
        /// </summary>
        /// <param name="parameters">The filter and Search parameters. Found in the query string.</param>
        /// <returns>The records to be returned as OutboundPropertyDto's.</returns>
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<OutboundPropertyDto>> GetProperties(
            [FromQuery] PropertiesResourceParameters parameters = null)
        {
            var propertiesFromRepo = _propertyService.Get(parameters);

            return Ok(_mapper.Map<IEnumerable<OutboundPropertyDto>>(propertiesFromRepo));
        }
        /// <summary>
        /// Queries a single Property from the database with the specified ID.
        /// </summary>
        /// <param name="propertyId">The ID of the Property to be returned. Provided in the request route.
        /// </param>
        /// <returns>The record with the specified id as an OutboundPropertyDto.</returns>
        [HttpGet("{propertyId:length(24)}", Name = "GetPropertyById")]
        [HttpHead("{propertyId:length(24)}")]
        public ActionResult<OutboundPropertyDto> GetPropertyById(string propertyId)
        {
            var propertyFromRepo = _propertyService.Get(propertyId);
            if (propertyFromRepo == null) return NotFound();

            return Ok(_mapper.Map<OutboundPropertyDto>(propertyFromRepo));
        }
        /// <summary>
        /// Creates a new Property record in the database.
        /// </summary>
        /// <param name="property">The CreatePropertyDto used to create the entity. Provided in the body.
        /// <param name="userId">The ID of the user making the API calls. Provided in the header.</param>
        /// <returns>The newly created Property from the databse as an OutboundPropertyDto, and its location.</returns>
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
        /// <summary>
        /// Upserts a property record in the database. If updating, neglected fields are set to their default value (Null).
        /// </summary>
        /// <param name="propertyId">The ID of the Property to update. Provided in the Route.</param>
        /// <param name="property">The UpdatePropertyDto that holds the new field values. Provided in the body.</param>
        /// <param name="userId">The ID of the user making the api request. Provided in the Headers.</param>
        /// <returns>If it is created, the newly created Property from the databse as an OutboundPropertyDto is returned,
        /// as well as its location.</returns>
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

            _propertyService.Update(propertyFromRepo);

            return NoContent();
        }
        /// <summary>
        /// Updates a Property record in the database using a JSON patch document.
        /// </summary>
        /// <param name="propertyId">The ID of the record to be updated. Provided in the route.</param>
        /// <param name="patchDocument">The JSON patch document. Provided in the body.</param>
        /// <param name="userId">The ID of the user making the api calls. Provided in the header.</param>
        /// <returns>BadRequest if the record or user is not found. No content if successful.</returns>
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
            _propertyService.Update(propertyToPatch);

            return NoContent();
        }
        /// <summary>
        /// Removes a specific Property from the database.
        /// </summary>
        /// <param name="propertyId">The ID of the Property to be deleted. Provided in the route.</param>
        /// <param name="userId">The Id of the User making the API call. Provided in the header.</param>
        /// <returns>BadRequest if the record or user is not found. No content if successful.</returns>
        [HttpDelete("{propertyId:length(24)}}")]
        public IActionResult DeleteProperty(string propertyId,
            [FromHeader] string userId = null)
        {
            //this should also delete/cancel a properties showings.
            if (userId == null) return BadRequest(new { message = "A UserID is required to delete property records." });

            var propertyToDelete = _propertyService.Get(propertyId);
            if (propertyToDelete == null) return NotFound();

            if (propertyToDelete.OwnerID != userId) return BadRequest(new { message = "Only the owner of a property is allowed to delete its fields. " });
            
            _propertyService.Remove(propertyToDelete.Id);

            return NoContent();
        }
        /// <summary>
        /// Returns the methods supported at the resource endpoint.
        /// </summary>
        /// <returns>Returns an OK with the allowed request methods in the header.</returns>
        [HttpOptions]
        public IActionResult GetPropertiesResourceOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS,HEAD");
            return Ok();
        }
        /// <summary>
        /// Returns the methods supported at a unique record endpoint.
        /// </summary>
        /// <returns>Returns an OK with the allowed request methods in the header.</returns>
        [HttpOptions("{propertyId:length(24)}")]
        public IActionResult GetPropertiesRecordOptions()
        {
            Response.Headers.Add("Allow", "GET,PUT,PATCH,DELETE,HEAD");
            return Ok();
        }
    }
}
