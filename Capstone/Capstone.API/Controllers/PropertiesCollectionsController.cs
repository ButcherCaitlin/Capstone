using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Helpers;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone.API.Controllers
{
    [Route("api/properties/collections")]
    [ApiController]
    public class PropertiesCollectionsController : ControllerBase
    {
        private readonly PropertyService _propertyService;
        private readonly DatabaseService<User> _userService;
        private readonly IMapper _mapper;
        public PropertiesCollectionsController(PropertyService propertyService,
            DatabaseService<User> userService,
            IMapper mapper)
        {
            _propertyService = propertyService ??
                throw new ArgumentNullException(nameof(propertyService));
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Queries a collection of Properties from the database with the specified ID's.
        /// </summary>
        /// <param name="propertyIdsCollection">The ID's of the Properties to be returned. Provided in the request route.</param>
        /// <returns>The collection of records with the specified id's as an OutboundPropertyDto.</returns>
        [HttpGet("({propertyIdsCollection})", Name = "GetPropertiesCollection")]
        [HttpHead("({propertyIdsCollection})")]
        public ActionResult<IEnumerable<OutboundPropertyDto>> GetPropertyCollection(
        [FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<string> propertyIdsCollection)
        {
            if (propertyIdsCollection == null) return BadRequest();
            var propertyEntities = _propertyService.Get(propertyIdsCollection);
            if (propertyIdsCollection.Count() != propertyEntities.Count()) return NotFound();
            return Ok(_mapper.Map<IEnumerable<OutboundPropertyDto>>(propertyEntities));
        }
        /// <summary>
        /// Creates a new collection of Property records in the database.
        /// </summary>
        /// <param name="propertiesToAdd">The CreatePropertyDto's used to create the entities. Provided in the body.</param>
        /// <param name="userId">The ID of the user making the API calls. Provided in the header.</param>
        /// <returns>The newly created Properties from the databse as an OutboundPropertyDto's, and their location.</returns>
        [HttpPost]
        public ActionResult<IEnumerable<OutboundPropertyDto>> CreatePropertyCollection(
            IEnumerable<CreatePropertyDto> propertiesToAdd,
            [FromHeader]string userId)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to create property records." });

            IEnumerable<Property> propertiesAsEntity = _mapper.Map<IEnumerable<Property>>(propertiesToAdd);
            foreach (var property in propertiesAsEntity) property.OwnerID = userId;
            var propertiesFromRepo = _propertyService.Create(propertiesAsEntity);
            IEnumerable<OutboundPropertyDto> propertiesToReturn = _mapper.Map<IEnumerable<OutboundPropertyDto>>(propertiesFromRepo);
            var idsAsString = string.Join(",", propertiesToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetPropertiesCollection",
                new { propertyIdsCollection = idsAsString },
                propertiesToReturn);
        }
        /// <summary>
        /// Removes a set of Properties from the database.
        /// </summary>
        /// <param name="propertyIdsCollection">The ID's of the Properties to be deleted. Provided in the route.</param>
        /// <param name="userId">The Id of the User making the API call. Provided in the header.</param>
        /// <returns>BadRequest if the any record or user is not found. No content if successful.</returns>
        [HttpDelete("({propertyIdsCollection})")]
        public IActionResult DeletePropertyCollection()
        {
            return NotFound("THIS METHOD IS NOT CURRENTLY IMPLIMENTED");
        }

        /// <summary>
        /// Returns the methods supported at the resource endpoint.
        /// </summary>
        /// <returns>Returns an OK with the allowed request methods in the header.</returns>
        [HttpOptions]
        public IActionResult GetPropertiesCollectionsResourceOptions()
        {
            Response.Headers.Add("Allow", "POST,OPTIONS");
            return Ok();
        }
        /// <summary>
        /// Returns the methods supported at a unique record endpoint.
        /// </summary>
        /// <returns>Returns an OK with the allowed request methods in the header.</returns>
        [HttpOptions("{propertyId:length(24)}")]
        public IActionResult GetPropertiesCollectionsRecordOptions()
        {
            Response.Headers.Add("Allow", "GET,DELETE,HEAD,OPTIONS");
            return Ok();
        }
    }
}
