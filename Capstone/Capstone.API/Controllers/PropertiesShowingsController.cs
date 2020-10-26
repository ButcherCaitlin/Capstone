using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Capstone.API.Controllers
{
    [Route("api/properties/{propertyId}/showings")]
    [ApiController]
    public class PropertiesShowingsController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly PropertyService _propertyService;
        private readonly ShowingService _showingService;
        private readonly IMapper _mapper;

        public PropertiesShowingsController(UserService userService,
            PropertyService propertyService,
            ShowingService showingService,
            IMapper mapper)
        {
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
            _propertyService = propertyService ??
                throw new ArgumentNullException(nameof(propertyService));
            _showingService = showingService ??
                throw new ArgumentNullException(nameof(showingService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        //create a showing for a user and property
        [HttpPost]
        public ActionResult<CustomOutboundShowingDto> CreateShowingForPropertyAndUser(string propertyId,
            CreateShowingDto showing,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest("A UserID is required to create Showing records.");

            var propertyFromRepo = _propertyService.Get(propertyId);
            if (propertyFromRepo == null) return NotFound();

            var entityToAdd = _mapper.Map<Showing>(showing);
            entityToAdd.PropertyID = propertyId;
            entityToAdd.RealtorID = propertyFromRepo.OwnerID;
            entityToAdd.ProspectID = userId;

            var showingToReturnEntity = _showingService.Create(entityToAdd);

            return CreatedAtRoute("GetShowingById",
                new { showingId = showingToReturnEntity.Id },
                _mapper.Map<ProspectShowingDto>(showingToReturnEntity));
        }

        [HttpOptions]
        public IActionResult GetShowingsOptions()
        {
            Response.Headers.Add("Allow", "POST,OPTIONS");
            return Ok();
        }
    }
}
