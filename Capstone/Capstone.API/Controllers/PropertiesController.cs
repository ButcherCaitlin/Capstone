using System;
using System.Collections.Generic;
using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.API.Controllers
{
    //it woudl be helpful to have an api/properties/{propertyID}/nextavailableshowing
    [Route("api/users/{userId}/properties")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly PropertyService _propertyService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public PropertiesController(PropertyService propertyService,
            UserService userService,
            IMapper mapper)
        {
            _propertyService = propertyService ??
                throw new ArgumentNullException(nameof(propertyService));
            _userService = userService ??
                throw new ArgumentNullException(nameof(propertyService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PropertyDto>> GetPropertiesForUser(string userId)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null) return NotFound();
            var propertiesFromRepo = _propertyService.GetPropertiesForUser(userId);
            return Ok(_mapper.Map<IEnumerable<PropertyDto>>(propertiesFromRepo));
        }

        [HttpGet("{propertyId:length(24)}", Name = "GetPropertyForUser")]
        public ActionResult<PropertyDto> GetPropertyForUser(string userId, string propertyId)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null) return NotFound();
            var propertyFromRepo = _propertyService.Get(userId, propertyId);
            if (propertyFromRepo == null) return NotFound();
            return Ok(_mapper.Map<PropertyDto>(propertyFromRepo));
        }

        [HttpPost]
        public ActionResult<PropertyDto> CreateProperty(string userId, PropertyDto property)
        {
            property.OwnerID = userId;
            _propertyService.Create(_mapper.Map<Property>(property));
            return CreatedAtRoute("GetPropertyForUser", new { userId = property.OwnerID, propertyId = property.Id }, property);
        }
    }
}

//[HttpPost]
//public ActionResult<Property> Create(Property property)
//{
//    _propertyService.Create(property);
//    return CreatedAtRoute("GetProperty", new { id = property.Id.ToString() }, property);
//}

//[HttpPut("{id:length(24)}")]
//public IActionResult Update(string id, Property propertyIn)
//{
//    var property = _propertyService.Get(id);
//    if (property == null) return NotFound();
//    _propertyService.Update(id, propertyIn);
//    return NoContent();
//}

//[HttpDelete("{id:length(24)}")]
//public IActionResult Delete(string id)
//{
//    var property = _propertyService.Get(id);
//    if (property == null) return NotFound();
//    _propertyService.Remove(property.Id);
//    return NoContent();
//}
