using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Capstone.API.Controllers
{
    //you could create a new controller with the route api/bulkActions/users that creates the users and stores that change in a batch to be easily undone if needed.
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

        //This will get all users
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<OutboundUserDto>> GetUsers()
        {
            var usersFromRepo = _userService.Get();
            return Ok(_mapper.Map<IEnumerable<OutboundUserDto>>(usersFromRepo));
        }

        //this will get a specific user
        [HttpGet("{userId:length(24)}", Name = "GetUserById")]
        [HttpHead("{userId:length(24)}")]
        public ActionResult<OutboundUserDto> GetUser(string userId)
        {
            var user = _userService.Get(userId);
            if (user == null) return NotFound();
            return Ok(_mapper.Map<OutboundUserDto>(user));
        }

        //this will create a single user and return the correct locaiton
        [HttpPost]
        public ActionResult<OutboundUserDto> Create(CreateUserDto user)
        {
            var createdUser = _userService.Create(_mapper.Map<User>(user));
            return CreatedAtRoute("GetUserById", new { userId = createdUser.Id.ToString() }, _mapper.Map<OutboundUserDto>(createdUser));
        }

        [HttpPut("{userId:length(24)}")]
        public IActionResult UpdateUser(string userId, UpdateUserDto user)
        {
            var userFromRepo = _userService.Get(userId);
            if (userFromRepo == null)
            {//implement upsert
                var userToAdd = _mapper.Map<User>(user);
                userToAdd.Id = userId;
                _userService.Create(userToAdd);

                return CreatedAtRoute("GetUserById",
                    new { userId },
                    _mapper.Map<OutboundUserDto>(userToAdd));
            }

            _mapper.Map(user, userFromRepo);

            _userService.Update(userFromRepo.Id, userFromRepo);

            return NoContent();
        }

        [HttpPatch("{userId:length(24)}")]
        public IActionResult PartiallyUpdateUser(string userId,
            JsonPatchDocument<UpdateUserDto> patchDocument)
        {
            var userToPatch = _userService.Get(userId);
            if (userToPatch == null) return NotFound( new { message = "If you are trying to upsert a resource use PUT" });

            var userDtoToPatch = _mapper.Map<UpdateUserDto>(userToPatch);
            patchDocument.ApplyTo(userDtoToPatch, ModelState); //add validation

            if (!TryValidateModel(userDtoToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userDtoToPatch, userToPatch);
            _userService.Update(userToPatch.Id, userToPatch);

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetUsersOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS,HEAD");
            return Ok();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
