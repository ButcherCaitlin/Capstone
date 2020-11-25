using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Repositories;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;

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
        public ActionResult<CustomOutboundShowingDto> CreateShowingForPropertyAndUser(string propertyId,
            CreateShowingDto showing,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to create Showing records." });

            var propertyFromRepo = _dataService.GetProperty(propertyId);
            if (propertyFromRepo == null) return NotFound();

            var entityToAdd = _mapper.Map<Showing>(showing);
            entityToAdd.PropertyID = propertyId;
            entityToAdd.RealtorID = propertyFromRepo.OwnerID;
            entityToAdd.ProspectID = userId;

            var showingToReturnEntity = _dataService.Create(entityToAdd);

            return CreatedAtRoute("GetShowingById",
                new { showingId = showingToReturnEntity.Id },
                _mapper.Map<ProspectShowingDto>(showingToReturnEntity));
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
