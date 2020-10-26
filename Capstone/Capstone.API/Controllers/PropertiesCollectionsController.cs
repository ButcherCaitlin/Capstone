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
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public PropertiesCollectionsController(PropertyService propertyService,
            UserService userService,
            IMapper mapper)
        {
            _propertyService = propertyService ??
                throw new ArgumentNullException(nameof(propertyService));
            _userService = userService ??
                throw new ArgumentNullException(nameof(UserService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Returns a collection of properties specified by an array of their ids in the route.
        /// </summary>
        /// <param name="propertyIdsCollection">The ids to get the properties.</param>
        /// <returns>A collection of property objects.</returns>
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
        /// Creates multiple properties as a collection and returns the route to the collection
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="propertiesToAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<IEnumerable<OutboundPropertyDto>> CreatePropertyCollection(
            [FromHeader]string userId, IEnumerable<CreatePropertyDto> propertiesToAdd)
        {
            if (userId == null) return BadRequest("A UserID is required to create property records.");

            IEnumerable<Property> propertiesAsEntity = _mapper.Map<IEnumerable<Property>>(propertiesToAdd);
            foreach (var property in propertiesAsEntity) property.OwnerID = userId;
            var propertiesFromRepo = _propertyService.CreateMany(propertiesAsEntity);
            IEnumerable<OutboundPropertyDto> propertiesToReturn = _mapper.Map<IEnumerable<OutboundPropertyDto>>(propertiesFromRepo);
            var idsAsString = string.Join(",", propertiesToReturn.Select(a => a.Id));
            return CreatedAtRoute("GetPropertiesCollection",
                new { userId = userId, propertyIdCollection = idsAsString },
                propertiesToReturn);
        }

        [HttpOptions]
        public IActionResult GetPropertiesCollectionsOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS,HEAD");
            return Ok();
        }
    }
}
