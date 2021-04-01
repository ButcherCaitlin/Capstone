using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using Capstone.API.Utility;
using System.Collections.Generic;

namespace Capstone.API.Controllers
{
    [Route("api/properties/{propertyId}/showings")]
    [ApiController]
    public class PropertiesShowingsController : ControllerBase
    {
        private readonly DataService _dataService;
        private readonly IMapper _mapper;
        public PropertiesShowingsController(DataService dataService, IMapper mapper)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        /// <summary>
        /// Creates a new Showing record in the database.
        /// </summary>
        /// <param name="propertyId">The property ID to associate the showing with. Provided in the route.</param>
        /// <param name="showing">The CreateShwoingDto used to create the entity. Provided in the body.</param>
        /// <param name="userId">The ID of the user making the API calls. Provided in the header.</param>
        /// <returns>The newly created Showing from the databse as an OutboundShowingDto, and its location.</returns>
        [HttpPost]
        public ActionResult<OutboundShowingDto> CreateShowingForPropertyAndUser(string propertyId,
            CreateShowingDto showing,
            [FromHeader] string userId = null)
        {
            //first make sure that the showing, user, and property exist by getting them from the DB.
            if (userId == null) return BadRequest(new { message = "A UserID is required to create Showing records." });

            List<object> participants = new List<object>();
            var propertyFromRepo = _dataService.GetProperty(propertyId);
            if (propertyFromRepo == null) return NotFound("The property ID provided could not be found.");
            participants.Add(propertyFromRepo);
            var userFromRepo = _dataService.GetUser(userId);
            if (userFromRepo == null) return NotFound("The User ID provided could not be found.");
            participants.Add(userFromRepo);
            var hostFromRepo = _dataService.GetUser(propertyFromRepo.OwnerID);
            if (hostFromRepo == null) return NotFound("The Owner ID provided could not be found.");
            participants.Add(hostFromRepo);

            var entityToAdd = _mapper.Map<Showing>(showing);
            entityToAdd.PropertyID = propertyId;
            entityToAdd.RealtorID = propertyFromRepo.OwnerID;
            entityToAdd.ProspectID = userId;

            foreach (var p in participants)
            {
                if (p is Property property && property.CustomAvailability == false)
                    property.Availability = AvailibilityUtility.CreateDefaultAvailibility().Result;
                if (p is User user && user.CustomAvailability == false)
                    user.Availability = AvailibilityUtility.CreateDefaultAvailibility().Result;
            }


            //then make sure that each of the entities are available at that time, and add the event to each objects availability.
            bool success = AvailibilityUtility.NoConflict(participants, entityToAdd).Result;

            //if each entity is available at that time create the showing object, and add update the avail on each 
            //entity.

            if (success)
            {
                //create the new showing object and push it to the database.
                var showingToReturnEntity = _dataService.Create(entityToAdd);

                //update the objects with the new availability
                _dataService.Update(propertyFromRepo);
                _dataService.Update(userFromRepo);
                _dataService.Update(hostFromRepo);

                //return the route
                return CreatedAtRoute("GetShowingById",
                    new { showingId = showingToReturnEntity.Id },
                    _mapper.Map<OutboundShowingDto>(showingToReturnEntity));
            } 
            else
            {
                return Conflict("The showing time selected was not available.");
            }

            //if each entity is not available return a failed response.
        }
        /// <summary>
        /// Returns the methods supported at the resource endpoint.
        /// </summary>
        /// <returns>Returns an OK with the allowed request methods in the header.</returns>
        [HttpOptions]
        public IActionResult GetShowingsOptions()
        {
            Response.Headers.Add("Allow", "POST,OPTIONS");
            return Ok();
        }
    }
}
