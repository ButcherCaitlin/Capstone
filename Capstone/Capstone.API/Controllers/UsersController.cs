using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Capstone.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly PropertyService _propertyService;
        private readonly ShowingService _showingService;
        private readonly IMapper _mapper;

        public UsersController(UserService userService,
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
        #region User Controllers
        //This will get all users
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var usersFromRepo = _userService.Get();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(usersFromRepo));
        }
        //this will get a specific user
        [HttpGet("{userId:length(24)}", Name = "GetUser")]
        public ActionResult<UserDto> GetUser(string userId)
        {
            var user = _userService.Get(userId);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<UserDto>(user));
        }
        //this will create a single user and return the correct locaiton
        [HttpPost]
        public ActionResult<UserDto> Create(UserDto user)
        {
            var createdUser = _userService.Create(_mapper.Map<User>(user));
            return CreatedAtRoute("GetUser", new { userId = createdUser.Id.ToString() }, _mapper.Map<UserDto>(createdUser));
        }
        #endregion
        #region User/Property Controllers
        //this will get all of the properties a specific user owns.
        [HttpGet("{userId:length(24)}/properties", Name = "GetPropertiesForUser")]
        public ActionResult<IEnumerable<PropertyDto>> GetPropertiesForUser(string userId)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null) return NotFound();
            var propertiesFromRepo = _propertyService.GetPropertiesForUser(userId);
            return Ok(_mapper.Map<IEnumerable<PropertyDto>>(propertiesFromRepo));
        }
        //this will create a batch of properties for a user
        [HttpPost("{userId:length(24)}/properties")]
        public ActionResult<IEnumerable<PropertyDto>> CreatePropertiesForUser(string userId, IEnumerable<PropertyDto> propertiesToAdd)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null) return NotFound();

            IEnumerable<Property> propertiesAsEntity = _mapper.Map<IEnumerable<Property>>(propertiesToAdd);
            foreach (var property in propertiesAsEntity) property.OwnerID = userId;

            var propertiesFromRepo = _propertyService.CreateMany(propertiesAsEntity);
            IEnumerable<PropertyDto> propertiesToReturn = _mapper.Map<IEnumerable<PropertyDto>>(propertiesFromRepo);

            return CreatedAtRoute("GetPropertiesForUser", new { userId = userId }, propertiesToReturn);
        }
        #endregion
        #region User/Property/Showing Controllers
        //get all showings for a user
        [HttpGet("{userId:length(24)}/showings", Name = "GetShowingsForUser")]
        public ActionResult<IEnumerable<ShowingDto>> GetShowingsForUser(string userId)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null) return NotFound();
            var showingsFromRepo = _showingService.GetShowingsForUser(userId);

            var showingsToReturn = new List<ShowingDto>();
            foreach (var showing in showingsFromRepo)
            {
                if (showing.RealtorID == userId)
                {
                    showingsToReturn.Add(_mapper.Map<ShowingForRealtorDto>(showing));
                }
                else if (showing.RealtorID != userId)
                {
                    showingsToReturn.Add(_mapper.Map<ShowingForProspectDto>(showing));
                }
            }

            return Ok(showingsToReturn);
        }
        //get a specific showing for the user
        [HttpGet("{userId:length(24)}/showings/{showingId:length(24)}", Name = "GetShowingForUser")]
        public ActionResult<ShowingDto> GetShowingForUser(string userId, string showingId)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null) return NotFound();
            var showingFromRepo = _showingService.Get(showingId);
            if (showingFromRepo == null) return NotFound();

            if (userId == showingFromRepo.ProspectID)
            {
                return Ok(_mapper.Map<ShowingForProspectDto>(showingFromRepo));
            } 
            else if (userId == showingFromRepo.RealtorID)
            {
                return Ok(_mapper.Map<ShowingForRealtorDto>(showingFromRepo));
            }
            else
            {
                return NotFound();
            }
        }
        //create a showing for a user and property
        [HttpPost("{userId:length(24)}/properties/{propertyId:length(24)}/showings")]
        public ActionResult<ShowingDto> CreateShowingForPropertyAndUser(string userId, string propertyId, ShowingDto showing)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null) return NotFound();
            var propertyFromRepo = _propertyService.Get(propertyId);
            if (propertyFromRepo == null) return NotFound();

            var entityToAdd = _mapper.Map<Showing>(showing);
            entityToAdd.PropertyID = propertyId;
            entityToAdd.RealtorID = propertyFromRepo.OwnerID;
            entityToAdd.ProspectID = userId;
            
            var showingToReturnEntity = _showingService.Create(entityToAdd);

            return CreatedAtRoute("GetShowingForUser",
                new { userId = userId, showingId = showingToReturnEntity.Id },
                _mapper.Map<ShowingForProspectDto>(showingToReturnEntity));

        }
        #endregion
    }
}
