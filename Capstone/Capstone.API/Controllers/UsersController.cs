using System;
using System.Collections.Generic;
using AutoMapper;
using Capstone.API.Entities;
using Capstone.API.Models;
using Capstone.API.ResourceParameters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Capstone.API.Services;
using Capstone.API.Utility;
using System.Threading.Tasks;
using System.Linq;

namespace Capstone.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataService _dataService;
        private readonly IMapper _mapper;
        public UsersController(DataService dataService,
            IMapper mapper)
        {
            _dataService = dataService ??
                throw new ArgumentNullException(nameof(dataService));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<OutboundUserDto>>> GetUsers()
        {
            var usersFromRepo = _dataService.GetUsers();
            var usersOutbound = _mapper.Map<IEnumerable<OutboundUserDto>>(usersFromRepo);

            var usersWithoutCustomAvailibility = usersOutbound.Where(u => u.CustomAvailability == false);

            foreach (OutboundUserDto user in usersWithoutCustomAvailibility)
            {
                user.Availability = await AvailibilityUtility.CreateDefaultAvailibility();
            }

            return Ok(usersOutbound);

            //return Ok(_mapper.Map<IEnumerable<OutboundUserDto>>(usersFromRepo));
        }
        [HttpGet("{userId:length(24)}", Name = "GetUserById")]
        [HttpHead("{userId:length(24)}")]
        public async Task<ActionResult<OutboundUserDto>> GetUserById(string userId)
        {
            var userFromRepo = _dataService.GetUser(userId);
            if (userFromRepo == null) return NotFound();

            var userOutbound = _mapper.Map<OutboundUserDto>(userFromRepo);

            if (userOutbound.CustomAvailability == false)
                userOutbound.Availability = await AvailibilityUtility.CreateDefaultAvailibility();

            return Ok(userOutbound);
        }
        [HttpPost]
        public ActionResult<OutboundUserDto> CreateUser(CreateUserDto user,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to create records." });

            var userToAdd = _mapper.Map<User>(user);

            if (userToAdd.Availability == null) userToAdd.CustomAvailability = false;

            var createdUser = _dataService.Create(userToAdd);

            return CreatedAtRoute("GetUserById",
                new { userId = createdUser.Id.ToString() },
                _mapper.Map<OutboundPropertyDto>(createdUser));
        }
        [HttpPut("{userRecordId:length(24)}")]
        public IActionResult UpdateUser(string userRecordId, UpdateUserDto user,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to Modify records." });

            var userFromRepo = _dataService.GetUser(userRecordId);
            if (userFromRepo == null)
            {
                var userToAdd = _mapper.Map<User>(user);
                userToAdd.Id = userRecordId;

                _dataService.Create(userToAdd);

                return CreatedAtRoute("GetUserById",
                    new { userRecordId },
                    _mapper.Map<OutboundPropertyDto>(userToAdd));
            }

            _mapper.Map(user, userFromRepo);

            _dataService.Update(userFromRepo);

            return NoContent();
        }
        [HttpPatch("{userRecordId:length(24)}")]
        public IActionResult PartiallyUpdateUser(string userRecordId,
            JsonPatchDocument<UpdateUserDto> patchDocument,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to Modify records." });

            var userToPatch = _dataService.GetUser(userRecordId);
            if (userToPatch == null) return NotFound(new { message = "If you are trying to upsert a resource use PUT" });

            var userDtoToPatch = _mapper.Map<UpdateUserDto>(userToPatch);
            patchDocument.ApplyTo(userDtoToPatch, ModelState);

            if (!TryValidateModel(userDtoToPatch)) return ValidationProblem(ModelState);

            _mapper.Map(userDtoToPatch, userToPatch);
            _dataService.Update(userToPatch);

            return NoContent();
        }
        [HttpDelete("{userRecordId:length(24)}")]
        public IActionResult DeleteUser(string userRecordId,
            [FromHeader] string userId = null)
        {
            if (userId == null) return BadRequest(new { message = "A UserID is required to delete property records." });

            var userToDelete = _dataService.GetUser(userRecordId);
            if (userToDelete == null) return NotFound();

            _dataService.RemoveUser(userToDelete.Id);

            return NoContent();
        }
        [HttpOptions]
        public IActionResult GetUsersResourceOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS,HEAD");
            return Ok();
        }
        [HttpOptions("{userId:length(24)}")]
        public IActionResult GetUsersResourceRecordOptions()
        {
            Response.Headers.Add("Allow", "GET,PUT,PATCH,DELETE,HEAD");
            return Ok();
        }
    }
}
